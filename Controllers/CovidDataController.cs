using System;
using System.Collections.Generic;
using System.Linq;
using COVID_19.Data.Repository;
using COVID_19.Models;
using COVID_19.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace COVID_19.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CovidDataController : ControllerBase
    {
        public ICovidDataRepository _covidDataRepository;
        public ICountryRepository _countryRepository;

        public CovidDataController(ICovidDataRepository covidDataRepository, ICountryRepository countryRepository)
        {
            _covidDataRepository = covidDataRepository;
            _countryRepository = countryRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CovidDataViewModel>> Index()
        {
            var allCovidData = _covidDataRepository.AllCovidData().ToList();
            return Ok(allCovidData);
        }

        //[HttpGet]
        //public ActionResult<IEnumerable<Country>> Index()
        //{
        //    var allCovidData = _countryRepository.AllCountryData().ToList();
        //    return Ok(allCovidData);
        //}
    }
}