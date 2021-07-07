using System.Collections.Generic;
using COVID_19.Models;
using COVID_19.Models.ViewModels;

namespace COVID_19.Data.Repository
{
    public interface ICovidVaccineRepository
    {
        IEnumerable<CovidVaccineData> AllCovidVaccineData { get; }
        List<VaccineGridDataViewModel> VaccineGridData();
        List<VaccineGridDataViewModel> VaccineCountryGridData(string countryUI);
        List<VaccineGraphDataViewModel> VaccineGraphData(string countryUI);
    }
}