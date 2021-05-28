using COVID_19.CoreApiClient;
using COVID_19.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace COVID_19.Data.Repository
{
    public class InformationRepository : IInformationRepository
    {
        public INewsAPIClient _newsAPIClient;
        public ICountryRepository _countryRepository;
        public InformationRepository(INewsAPIClient newsAPIClient, ICountryRepository countryRepository)
        {
            _newsAPIClient = newsAPIClient;
            _countryRepository = countryRepository;
        }
        public IEnumerable<NewsArticle> GetTopCovidNewsCountry(string country)
        {
            var countryIso = country == "World"? "nil" : _countryRepository.AllCountryData().Where(x => x.country_name == country).FirstOrDefault().iso2;
            var covidCountryNews = _newsAPIClient.FetchCovidNews(countryIso);
            return covidCountryNews;
        }
    }
}
