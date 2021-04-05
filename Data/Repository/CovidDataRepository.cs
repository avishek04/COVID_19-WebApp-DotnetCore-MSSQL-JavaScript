using System;
using System.Collections.Generic;
using System.Linq;
using COVID_19.CoreApiClient;
using COVID_19.Models;
using COVID_19.Models.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace COVID_19.Data.Repository
{
    public class CovidDataRepository : ICovidDataRepository
    {
        private AppDbContext _appDbContext;
        private ICovidDataClient _covidDataClient;
        private ICountryRepository _countryRepository;
        private IServiceScopeFactory _serviceScopeFactory;

        public CovidDataRepository(AppDbContext appDbContext, ICovidDataClient covidDataClient, ICountryRepository countryRepository, IServiceScopeFactory serviceScopeFactory)
        {
            _appDbContext = appDbContext;
            _covidDataClient = covidDataClient;
            _countryRepository = countryRepository;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IEnumerable<CovidData> AllCountryCovidData
        {
            get
            {
                return _appDbContext.covidcountrydata;
            }
        }

        public IEnumerable<CovidDataViewModel> AllCovidData()
        {
            UpdateAllCovidData();
            var allCovidData = from covidData in AllCountryCovidData
                               join country in _countryRepository.AllCountryData()
                               on covidData.country_id equals country.id
                               select new CovidDataViewModel
                               {
                                   country_name = country.country_name,
                                   iso2 = country.iso2,
                                   iso3 = country.iso3,
                                   report_date = covidData.report_date,
                                   db_update_date = covidData.db_update_date,
                                   confirmed_cases = covidData.confirmed_cases,
                                   death_cases = covidData.death_cases,
                                   recovered = covidData.recovered,
                                   active_cases = covidData.active_cases
                               };

            return allCovidData;
        }

        public void UpdateAllCovidData()
        {
            if (AllCountryCovidData.ToList().Count() < 1)
            {
                var startDate = new DateTime(2020, 1, 22);
                var endDate = DateTime.UtcNow;
                var allDates = GetAllDates(startDate, endDate);
                AddCovidData(allDates);
            }
            var reportDateList = AllCountryCovidData.Select(data => data.report_date)
                                                    .Distinct()
                                                    .ToList();
            var lastAddedDate = reportDateList.Max();
            var todayDate = DateTime.UtcNow;
            var yesterdayDate = todayDate.AddDays(-1);
            var lastWeekDate = todayDate.AddDays(-8);
            var twoWeekBackDate = DateTime.UtcNow.AddDays(-15);

            if (lastAddedDate < yesterdayDate)
            {
                var startDate = lastAddedDate.AddDays(1);
                var endDate = yesterdayDate;
                var allDates = GetAllDates(startDate, endDate);
                AddCovidData(allDates);
            }

            var lastWeekRecordsToUpdate = AllCountryCovidData.Where(data => data.report_date.Date > lastWeekDate.Date
                                                              && data.db_update_date.Date < yesterdayDate.Date)
                                                             .Select(data => data.report_date.Date)
                                                             .Distinct()
                                                             .ToList();
            var oldRecordsToUpdate = AllCountryCovidData.Where(data => data.report_date.Date < lastWeekDate.Date
                                                         && data.db_update_date.Date < twoWeekBackDate.Date)
                                                        .Select(data => data.report_date.Date)
                                                        .Distinct()
                                                        .ToList();

            if (lastWeekRecordsToUpdate.Any())
            {
                UpdateCovidData(lastWeekRecordsToUpdate);
            }

            if (oldRecordsToUpdate.Any())
            {
                UpdateCovidData(oldRecordsToUpdate);
            }
        }

        public async void AddCovidData(List<DateTime> dateList)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var allCountry = _countryRepository.AllCountryData();
                foreach (var date in dateList)
                {
                    var newCovidData = await _covidDataClient.FetchCovidDataAsync(date);
                    var covidData = new List<CovidData>();

                    foreach(var data in newCovidData)
                    {
                        var singleCountryData = new CovidData();
                        singleCountryData.country_id = allCountry.Where(x => x.country_name == data.CountryRegionJH
                                                                          || x.iso2 == data.CountryRegionJH.Substring(0, 2)
                                                                          || x.iso3 == data.CountryRegionJH.Substring(0, 3))
                                                                        .FirstOrDefault().id;
                        singleCountryData.report_date = date;
                        singleCountryData.db_update_date = DateTime.UtcNow;
                        singleCountryData.confirmed_cases = data.ActiveCasesJH;
                        singleCountryData.death_cases = data.DeathCasesJH;
                        singleCountryData.recovered = data.RecoveredJH;
                        singleCountryData.active_cases = data.ActiveCasesJH; //change the code to get Active Cases for old date

                        covidData.Add(singleCountryData);
                    }
                    dbContext.covidcountrydata.AddRange(covidData);
                }
                dbContext.SaveChanges();
            }
        }

        public async void UpdateCovidData(List<DateTime> dateList)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var allCountry = _countryRepository.AllCountryData();
                foreach (var date in dateList)
                {
                    var updateCovidData = await _covidDataClient.FetchCovidDataAsync(date);

                    foreach (var data in updateCovidData)
                    {
                        var singleCountryUpdate = (from covidData in dbContext.covidcountrydata
                                                  join countryData in allCountry
                                                  on covidData.country_id equals countryData.id
                                                  where covidData.report_date == date && countryData.country_name == data.CountryRegionJH
                                                  select covidData).FirstOrDefault();

                        if (singleCountryUpdate != null)
                        {
                            singleCountryUpdate.report_date = date;
                            singleCountryUpdate.db_update_date = DateTime.UtcNow;
                            singleCountryUpdate.confirmed_cases = data.ConfirmedCasesJH;
                            singleCountryUpdate.death_cases = data.DeathCasesJH;
                            singleCountryUpdate.recovered = data.RecoveredJH;
                            singleCountryUpdate.active_cases = data.ActiveCasesJH;
                        }
                    }
                }
                dbContext.SaveChanges();
            }
        }

        public List<DateTime> GetAllDates(DateTime startDate, DateTime endDate)
        {
            List<DateTime> allDates = new List<DateTime>();
            var start = startDate.Date;
            var end = endDate.Date;
            for (DateTime i = start; i <= end; i = i.AddDays(1))
            {
                allDates.Add(i);
            }
            return allDates;
        }

        //public async void AddCovidCountryDataAsync()
        //{
        //    var startDate = new DateTime(2020, 1, 22);
        //    var endDate = DateTime.UtcNow;
        //    var dateList = GetAllDates(startDate, endDate);

        //    foreach (var date in dateList)
        //    {
        //        var dateFormat = date.ToString("MM-dd-yyyy").Trim();
        //        var newRecord = await _covidDataClient.FetchCovidCountryDataAsync(dateFormat);

        //        foreach (var record in newRecord)
        //        {
        //            record.Last_Update_Date = DateTime.UtcNow;
        //        }
        //        using (var scope = _serviceScopeFactory.CreateScope())
        //        {
        //            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        //            dbContext.CovidCountryData.AddRange(newRecord);
        //            dbContext.SaveChanges();
        //        }
        //    }
        //}

        

        //public async void GetCovidDataFromJHAsync(List<CovidData> countryCovidData)
        //{
        //    try
        //    {
        //        var startDate = new DateTime(2020, 1, 22);
        //        var todaysDate = DateTime.UtcNow.Date;
        //        var oneDayBefore = DateTime.UtcNow.AddDays(-1);
        //        var pastRecordsToUpdate = countryCovidData.Where(x => x.Last_Update_Date <= oneDayBefore);
        //        var datesToUpdate = pastRecordsToUpdate.Select(x => x.Report_Date).Distinct().ToArray();

        //        if (pastRecordsToUpdate.Count() > 0)
        //        {
        //            foreach (var date in datesToUpdate)
        //            {
        //                var newRecord = await _covidDataClient.FetchCovidDataAsync(date);

        //                using (var scope = _serviceScopeFactory.CreateScope())
        //                {
        //                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        //                    var recordsToUpdate = dbContext.CovidCountryData.Where(x => x.Report_Date == date);
        //                    foreach (var record in recordsToUpdate)
        //                    {
        //                        var newData = newRecord.Where(x => x.Report_Date == record.Report_Date && x.Combined_Key == record.Combined_Key).FirstOrDefault();
        //                        record.ConfirmedCases = newData.ConfirmedCases;
        //                        record.Deaths = newData.Deaths;
        //                        record.Recovered = newData.Recovered;
        //                        record.ActiveCases = newData.ActiveCases;
        //                    }
        //                    dbContext.SaveChanges();
        //                }
        //            }
        //        }
        //        var reportDateInDb = countryCovidData.Select(x => x.Report_Date).Distinct().ToArray();
        //        var dateToAdd = new List<DateTime>();
        //        for (var i = startDate; i <= todaysDate; i = i.AddDays(1))
        //        {
        //            if (!reportDateInDb.Contains(i))
        //            {
        //                dateToAdd.Add(i);
        //            }
        //        }
        //        using (var scope = _serviceScopeFactory.CreateScope())
        //        {
        //            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        //            foreach (var date in dateToAdd)
        //            {
        //                var newRecord = await _covidDataClient.FetchCovidDataAsync(date);
        //                dbContext.CovidCountryData.AddRange(newRecord);
        //            }
        //            dbContext.SaveChanges();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message);
        //    }
        //}
    }
}