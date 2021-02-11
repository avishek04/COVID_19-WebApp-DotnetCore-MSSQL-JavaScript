using System;
using System.Collections.Generic;
using System.Linq;
using COVID_19.Data.Repository;
using COVID_19.Models;
using Microsoft.AspNetCore.Mvc;

namespace COVID_19.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CovidDataController : ControllerBase
    {
        public ICovidDataRepository _covidDataRepository;

        public CovidDataController(ICovidDataRepository covidDataRepository)
        {
            _covidDataRepository = covidDataRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CovidCountryData>> Index()
        {
            var allCountryData = _covidDataRepository.AllCountryData.ToList();
            return Ok(allCountryData);
        }
    }
}