using System.Collections.Generic;
using COVID_19.Models;

namespace COVID_19.Data.Repository
{
    public interface ICovidDataRepository
    {
        IEnumerable<CovidCountryData> AllCountryData { get; }
        void UpdateCovidCountryData();
    }
}