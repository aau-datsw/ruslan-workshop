using System;
using System.Collections.Generic;
using System.Linq;
using MarketAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace MarketAPI.Controllers
{
    [Route("")]
    [ApiController]
    public class MarketController : ControllerBase
    {
        private MarketAPIContext _dbContext;
        private IHostingEnvironment _hostingEnvironment;

        public MarketController(MarketAPIContext dbContext, IHostingEnvironment hostingEnvironment)
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
                    System.Console.WriteLine(company);
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
    }
}