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

        [HttpGet]
        [Route("all")]
        public ActionResult<IEnumerable<Person>> GetAllPersons()
        {
            return _dbContext.Persons.ToList();
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Person> GetPersonById(int id)
        {
            var person = _dbContext.Persons.FirstOrDefault(p => p.Id == id);
            if (person == null) 
                return NotFound($"Could not find a person with ID {id}.");
            return Ok(person);
        }
    }
}