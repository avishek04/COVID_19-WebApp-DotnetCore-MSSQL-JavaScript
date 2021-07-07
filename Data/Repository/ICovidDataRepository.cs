using System;
using System.Collections.Generic;
using COVID_19.Models;
using COVID_19.Models.ViewModels;

namespace COVID_19.Data.Repository
{
    public interface ICovidDataRepository
    {
        IEnumerable<CovidData> AllCountryCovidData { get; }
        List<DateTime> GetAllDates(DateTime startDate, DateTime endDate);
        List<CovidGridDataViewModel> CovidGridData();
        List<CovidGridDataViewModel> CovidCountryGridData(string countryUI);
        List<CovidGraphDataViewModel> CovidGraphData(string countryUI);
    }
}