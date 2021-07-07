using System;
using System.Collections.Generic;
using System.Linq;
using COVID_19.Data.Repository;
using COVID_19.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace COVID_19.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CovidDataController : ControllerBase
    {
        public ICovidDataRepository _covidDataRepository;
        public ICountryRepository _countryRepository;
        private readonly IMemoryCache _memoryCache;

        public CovidDataController(ICovidDataRepository covidDataRepository, ICountryRepository countryRepository, IMemoryCache memoryCache)
        {
            _covidDataRepository = covidDataRepository;
            _countryRepository = countryRepository;
            _memoryCache = memoryCache;
        }

        [HttpGet("{country}")]
        public ActionResult<List<CovidGridDataViewModel>> CovidCountryGridData(string country)
        {
            List<CovidGridDataViewModel> covidCountryGridData = new List<CovidGridDataViewModel>();
            bool isExist = _memoryCache.TryGetValue("CovidCountryGridData_" + country, out covidCountryGridData);

            if (!isExist)
            {
                covidCountryGridData = _covidDataRepository.CovidCountryGridData(country).ToList();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));

                _memoryCache.Set("CovidCountryGridData_" + country, covidCountryGridData, cacheEntryOptions);
            }
            return Ok(covidCountryGridData);
        }

        [HttpGet]
        public ActionResult<List<CovidGridDataViewModel>> CovidGridData()
        {
            List<CovidGridDataViewModel> covidGridData = new List<CovidGridDataViewModel>();
            bool isExist = _memoryCache.TryGetValue("CovidGridData", out covidGridData);

            if (!isExist)
            {
                covidGridData = _covidDataRepository.CovidGridData().ToList();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));

                _memoryCache.Set("CovidGridData", covidGridData, cacheEntryOptions);
            }
            return Ok(covidGridData);
        }

        [HttpGet("{country}")]
        public ActionResult<List<CovidGraphDataViewModel>> CovidGraphData(string country)
        {
            List<CovidGraphDataViewModel> covidGraphData = new List<CovidGraphDataViewModel>();
            bool isExist = _memoryCache.TryGetValue("CovidGraphData_" + country, out covidGraphData);

            if (!isExist)
            {
                covidGraphData = _covidDataRepository.CovidGraphData(country).ToList();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));

                _memoryCache.Set("CovidGraphData_" + country, covidGraphData, cacheEntryOptions);
            }
            return Ok(covidGraphData);
        }
    }
}