using System;
using System.Collections.Generic;
using System.Linq;
using COVID_19.CoreApiClient;
using COVID_19.Models;

namespace COVID_19.Data.Repository
{
    public class CovidDataRepository : ICovidDataRepository
    {
        public AppDbContext _appDbContext;
        public ICovidDataClient _covidDataClient;

        public CovidDataRepository(AppDbContext appDbContext, ICovidDataClient covidDataClient)
        {
            _appDbContext = appDbContext;
            _covidDataClient = covidDataClient;
        }

        public IEnumerable<CovidCountryData> AllCountryData
        {
            get
            {
                if (_appDbContext.CovidCountryData.Where(x => x.Last_Update >= DateTime.Now.AddDays(-1)).Count() > 0)
                {
                    return _appDbContext.CovidCountryData;
                }
                else
                {
                    UpdateCovidCountryData();
                    return _appDbContext.CovidCountryData;
                }
            }
        }


        public void UpdateCovidCountryData()
        {
            _covidDataClient.UpdateLatestCovidCountryDataAsync();
        }
    }
}
