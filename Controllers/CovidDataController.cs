using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COVID_19.CoreApiClient;
using COVID_19.Data.Repository;
using COVID_19.Models;
using Microsoft.AspNetCore.Mvc;

namespace COVID_19.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CovidDataController : Controller
    {
        public CovidDataRepository _covidDataRepository;

        public CovidDataController(CovidDataRepository covidDataRepository)
        {
            _covidDataRepository = covidDataRepository;
        }

        [HttpGet]
        public IEnumerable<CovidCountryData> Index()
        {
            var allCountryData = _covidDataRepository.AllCountryData;
            return allCountryData;
        }
    }
}