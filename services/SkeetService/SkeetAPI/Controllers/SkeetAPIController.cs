using System;
using System.Collections.Generic;
using System.Linq;
<<<<<<< HEAD:services/MarketService/MarketAPI/Controllers/MarketController.cs
using MarketAPI.Generation;
using MarketAPI.Models;
=======
using SkeetAPI.Models;
>>>>>>> origin:services/SkeetService/SkeetAPI/Controllers/SkeetAPIController.cs
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace SkeetAPI.Controllers
{
    [Route("")]
    [ApiController]
    public class MarketController : ControllerBase
    {
        private SkeetAPIContext _dbContext;
        private IHostingEnvironment _hostingEnvironment;

        public MarketController(SkeetAPIContext dbContext, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("companies/add-range")]
        public ActionResult AddCompanies(IEnumerable<Company> companies)
        {
            try
            {
                foreach (var company in companies)
                {
                    _dbContext.Companies.Add(company);
                    _dbContext.SaveChanges();
                }
                return Ok();
            }
            catch (Exception e)
            {
                if (_hostingEnvironment.IsDevelopment())
                    return BadRequest(e.Message);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("companies/all")]
        public ActionResult<IEnumerable<Company>> GetAllCompanies()
        {
            try
            {
                return _dbContext.Companies;
            }
            catch (Exception e)
            {
                if (_hostingEnvironment.IsDevelopment())
                    return BadRequest(e.Message);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("generate")]
        public ActionResult<IEnumerable<(int x, int y)>> GetMarketData(int companyId, int from, int to)
        {
            try
            {
                var company = _dbContext.Companies.FirstOrDefault(o => o.Id == companyId);
                if (company == null) 
                    return BadRequest($"Could not find a company with ID {companyId}.");

                var marketGenerator = new MarketGenerator(company);
                return Ok(marketGenerator.GenerateMarketChanges(from, to));

            }
            catch (Exception e)
            {
                if (_hostingEnvironment.IsDevelopment())
                    return BadRequest(e.Message);
                return BadRequest($"Something went wrong while generating market data for company {companyId}. Please try again later.");
            }
        }
    }
}