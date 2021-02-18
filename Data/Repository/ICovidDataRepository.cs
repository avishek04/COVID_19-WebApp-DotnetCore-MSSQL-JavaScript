using System;
using System.Collections.Generic;
using COVID_19.Models;

namespace COVID_19.Data.Repository
{
    public interface ICovidDataRepository
    {
        public IEnumerable<CovidCountryData> AllCountryCovidData { get; }
        IEnumerable<CovidCountryData> GetAllCovidData();
        IEnumerable<DateTime> GetAllDates(DateTime startingDate, DateTime endingDate);
        void UpdateCovidCountryDataAsync(List<CovidCountryData> countryCovidData);
    }
}