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
    public class VaccineDataController : ControllerBase
    {
        public ICovidVaccineRepository _covidVaccineRepository;
        public ICountryRepository _countryRepository;
        private readonly IMemoryCache _memoryCache;

        public VaccineDataController(ICovidVaccineRepository covidVaccineRepository, ICountryRepository countryRepository, IMemoryCache memoryCache)
        {
            _covidVaccineRepository = covidVaccineRepository;
            _countryRepository = countryRepository;
            _memoryCache = memoryCache;
        }

        [HttpGet("{country}")]
        public ActionResult<List<VaccineGridDataViewModel>> VaccineCountryGridData(string country)
        {
            List<VaccineGridDataViewModel> vaccineCountryGridData = new List<VaccineGridDataViewModel>();
            bool isExist = _memoryCache.TryGetValue("VaccineCountryGridData_" + country, out vaccineCountryGridData);

            if (!isExist)
            {
                vaccineCountryGridData = _covidVaccineRepository.VaccineCountryGridData(country).ToList();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));
                _memoryCache.Set("VaccineCountryGridData_" + country, vaccineCountryGridData, cacheEntryOptions);
            }

            return Ok(vaccineCountryGridData);
        }

        [HttpGet]
        public ActionResult<List<VaccineGridDataViewModel>> VaccineGridData()
        {
            List<VaccineGridDataViewModel> vaccineGridData = new List<VaccineGridDataViewModel>();
            bool isExist = _memoryCache.TryGetValue("VaccineGridData", out vaccineGridData);

            if (!isExist)
            {
                vaccineGridData = _covidVaccineRepository.VaccineGridData().ToList();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));
                _memoryCache.Set("VaccineGridData", vaccineGridData, cacheEntryOptions);
            }

            return Ok(vaccineGridData);
        }

        [HttpGet("{country}")]
        public ActionResult<List<VaccineGraphDataViewModel>> VaccineGraphData(string country)
        {
            List<VaccineGraphDataViewModel> vaccineGraphData = new List<VaccineGraphDataViewModel>();
            bool isExist = _memoryCache.TryGetValue("VaccineGraphData_" + country, out vaccineGraphData);

            if (!isExist)
            {
                vaccineGraphData = _covidVaccineRepository.VaccineGraphData(country).ToList();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));
                _memoryCache.Set("VaccineGraphData_" + country, vaccineGraphData, cacheEntryOptions);
            }

            return Ok(vaccineGraphData);
        }
    }
}
