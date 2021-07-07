using System;
using System.Collections.Generic;
using COVID_19.Data.Repository;
using COVID_19.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace COVID_19.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CountryDataController : ControllerBase
    {
        private ICountryRepository _countryRepository;
        private readonly IMemoryCache _memoryCache;

        public CountryDataController(ICountryRepository countryRepository, IMemoryCache memoryCache)
        {
            _countryRepository = countryRepository;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public ActionResult<List<Country>> CountryList()
        {
            List<Country> countryList = new List<Country>();
            bool isExist = _memoryCache.TryGetValue("CountryList", out countryList);

            if (!isExist)
            {
                countryList = _countryRepository.AllCountryData();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));
                _memoryCache.Set("CountryList", countryList, cacheEntryOptions);
            }

            return Ok(countryList);
        }
    }
}
