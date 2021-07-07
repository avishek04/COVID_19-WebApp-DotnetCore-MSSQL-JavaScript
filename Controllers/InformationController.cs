using COVID_19.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using COVID_19.Data.Repository;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace COVID_19.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class InformationController : ControllerBase
    {
        public IInformationRepository _informationRepository;
        private readonly IMemoryCache _memoryCache;

        public InformationController(IInformationRepository informationRepository, IMemoryCache memoryCache)
        {
            _informationRepository = informationRepository;
            _memoryCache = memoryCache;
        }

        //[HttpGet("{country}")]
        [HttpGet]
        public ActionResult<List<NewsArticle>> TopCovidNewsCountry()//(string country)
        {
            List<NewsArticle> countryCovidNews = new List<NewsArticle>();
            bool isExist = _memoryCache.TryGetValue("TopCovidNewsCountry", out countryCovidNews);

            if (!isExist || countryCovidNews.Count() < 5)
            {
                countryCovidNews = _informationRepository.GetTopCovidNewsCountry().ToList();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));
                _memoryCache.Set("TopCovidNewsCountry", countryCovidNews, cacheEntryOptions);
            }

            return Ok(countryCovidNews);
        }

        [HttpGet]
        public ActionResult<List<NewsArticle>> GetTopCovidNewsNYT()
        {
            List<NewsArticle> countryCovidNews = new List<NewsArticle>();
            bool isExist = _memoryCache.TryGetValue("GetTopCovidNewsNYT", out countryCovidNews);

            if (!isExist || countryCovidNews.Count() < 5)
            {
                countryCovidNews = _informationRepository.GetTopCovidNewsNYT().ToList();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));
                _memoryCache.Set("GetTopCovidNewsNYT", countryCovidNews, cacheEntryOptions);

            }

            return Ok(countryCovidNews);
        }
    }
}
