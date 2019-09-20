using System;
using System.Collections.Generic;
using System.Linq;
using YaAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace YaAPI.Controllers
{
    [Route("")]
    [ApiController]
    public class MarketController : ControllerBase
    {
        private YaAPIContext _dbContext;
        private IHostingEnvironment _hostingEnvironment;

        public MarketController(YaAPIContext dbContext, IHostingEnvironment hostingEnvironment)
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
    }
}