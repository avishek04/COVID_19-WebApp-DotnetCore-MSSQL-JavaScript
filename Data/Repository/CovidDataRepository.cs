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

        public IEnumerable<CovidGridDataViewModel> CovidGridData()
        {
            var covidGridData = new List<CovidGridDataViewModel>();
            var allCovidData = AllCovidData();
            //var covidCountryData = allCovidData.Where(data => data.country_name == country);
            var lastReportDate = allCovidData.OrderByDescending(x => x.report_date).FirstOrDefault().report_date;
            var dayBeforeLastReportDate = lastReportDate.AddDays(-1);
            var twoWeekBackReportDate = lastReportDate.AddDays(-15);
            var countryList = allCovidData.OrderBy(x => x.country_name).Select(x => x.country_name).Distinct().ToList();
            var todaysRecord = allCovidData.Where(x => x.report_date == lastReportDate).ToList();
            var yesterdaysRecord = allCovidData.Where(x => x.report_date == dayBeforeLastReportDate).ToList();
            var twoWeekBackRecord = allCovidData.Where(x => x.report_date == twoWeekBackReportDate).ToList(); 
            foreach (var country in countryList)
            {
                var todaysRecordCountry = todaysRecord.Where(x => x.country_name == country).FirstOrDefault();
                var yesterdaysRecordCountry = yesterdaysRecord.Where(x => x.country_name == country).FirstOrDefault();
                var twoWeekBackRecordCountry = twoWeekBackRecord.Where(x => x.country_name == country).FirstOrDefault();
                
                if (todaysRecordCountry == null)
                {
                    continue;
                }
                else
                {
                    covidGridData.Add(
                    new CovidGridDataViewModel
                    {
                        country_name = country,
                        totalCaseCount = Convert.ToInt64(todaysRecordCountry.confirmed_cases),
                        activeCaseCount = Convert.ToInt64(todaysRecordCountry.active_cases),
                        recoveredCaseCount = Convert.ToInt64(todaysRecordCountry.recovered),
                        lastDayCaseCount = Convert.ToInt64(todaysRecordCountry.confirmed_cases - yesterdaysRecordCountry.confirmed_cases),
                        lastTwoWeekCaseCount = Convert.ToInt64(todaysRecordCountry.confirmed_cases - twoWeekBackRecordCountry.confirmed_cases),
                        casesPerMillion = 0,
                        totalDeathCaseCount = Convert.ToInt64(todaysRecordCountry.death_cases)
                    });
                }
                
            }
            return covidGridData;
        }

        public IEnumerable<CovidGridDataViewModel> CovidCountryGridData(string country)
        {
            var covidGridData = new List<CovidGridDataViewModel>();
            var allCovidData = AllCovidData();
            var covidCountryData = allCovidData.Where(data => data.country_name == country);
            var lastReportDate = allCovidData.OrderByDescending(x => x.report_date).FirstOrDefault().report_date;
            var dayBeforeLastReportDate = lastReportDate.AddDays(-1);
            var twoWeekBackReportDate = lastReportDate.AddDays(-15);
            var todaysRecordCountry = covidCountryData.Where(x => x.report_date == lastReportDate)
                                                      .FirstOrDefault();
            var yesterdaysRecordCountry = covidCountryData.Where(x => x.report_date == dayBeforeLastReportDate)
                                                         .FirstOrDefault();
            var twoWeekBackRecordCountry = covidCountryData.Where(x => x.report_date == twoWeekBackReportDate)
                                                         .FirstOrDefault();
            var todaysRecordWorld = allCovidData.Where(x => x.report_date == lastReportDate).ToList();
            var yesterdaysRecordWorld = allCovidData.Where(x => x.report_date == dayBeforeLastReportDate).ToList();
            var twoWeekBackRecordWorld = allCovidData.Where(x => x.report_date == twoWeekBackReportDate).ToList();
            covidGridData.Add(
                new CovidGridDataViewModel
                {
                    country_name = country,
                    totalCaseCount = Convert.ToInt64(todaysRecordCountry.confirmed_cases),
                    activeCaseCount = Convert.ToInt64(todaysRecordCountry.active_cases),
                    recoveredCaseCount = Convert.ToInt64(todaysRecordCountry.recovered),
                    lastDayCaseCount = Convert.ToInt64(todaysRecordCountry.confirmed_cases - yesterdaysRecordCountry.confirmed_cases),
                    lastTwoWeekCaseCount = Convert.ToInt64(todaysRecordCountry.confirmed_cases - twoWeekBackRecordCountry.confirmed_cases),
                    casesPerMillion = 0,
                    totalDeathCaseCount = Convert.ToInt64(todaysRecordCountry.death_cases)
                }
            );
            covidGridData.Add(
                new CovidGridDataViewModel
                {
                    country_name = "World",
                    totalCaseCount = Convert.ToInt64(todaysRecordWorld.Sum(x => x.confirmed_cases)),
                    activeCaseCount = Convert.ToInt64(todaysRecordWorld.Sum(x => x.active_cases)),
                    recoveredCaseCount = Convert.ToInt64(todaysRecordWorld.Sum(x => x.recovered)),
                    lastDayCaseCount = Convert.ToInt64(todaysRecordWorld.Sum(x => x.confirmed_cases) - yesterdaysRecordWorld.Sum(x => x.confirmed_cases)),
                    lastTwoWeekCaseCount = Convert.ToInt64(todaysRecordWorld.Sum(x => x.confirmed_cases) - twoWeekBackRecordWorld.Sum(x => x.confirmed_cases)),
                    casesPerMillion = 0,
                    totalDeathCaseCount = Convert.ToInt64(todaysRecordWorld.Sum(x => x.death_cases))
                }
            );
            return covidGridData;
        }

        public IEnumerable<CovidGraphDataViewModel> CovidGraphData()
        {
            var covidGraphData = new List<CovidGraphDataViewModel>();
            var allCovidData = AllCovidData().Where(x => x.report_date > new DateTime(2020, 2, 1)).ToList();
            var lastDayDataSet = allCovidData.Select(x => new { x.report_date, x.country_name, x.confirmed_cases, x.death_cases });

            foreach (var covidData in allCovidData)
            {
                //var dayBefore = covidData.report_date.AddDays(-1);
                if (covidData.report_date != null && covidData.country_name != null)
                {
                    var dayBeforeRecord = lastDayDataSet.Where(x => x.report_date == covidData.report_date.AddDays(-1) && x.country_name == covidData.country_name)
                                                    .FirstOrDefault();
                    if (dayBeforeRecord != null)
                    {
                        if (covidData.confirmed_cases != null && covidData.active_cases != null &&
                        covidData.death_cases != null && dayBeforeRecord.confirmed_cases != null &&
                        dayBeforeRecord.death_cases != null)
                        {
                            covidGraphData.Add(new CovidGraphDataViewModel
                            {
                                country_name = covidData.country_name,
                                report_date = covidData.report_date,
                                new_cases = Convert.ToInt64(covidData.confirmed_cases - dayBeforeRecord.confirmed_cases),
                                active_cases = Convert.ToInt64(covidData.active_cases),
                                new_deaths = Convert.ToInt64(covidData.death_cases - dayBeforeRecord.death_cases)
                            });
                        }
                    }
                }
            }
            //covidGraphData.AddRange(covidGraphData);
            return covidGraphData;
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
            //if (AllCountryCovidData.ToList().Count() < 1)
            //{
            //    var startDate = new DateTime(2020, 1, 22);
            //    var endDate = DateTime.UtcNow.AddDays(-1);
            //    var allDates = GetAllDates(startDate, endDate);
            //    AddCovidData(allDates);
            //}
            var reportDateList = AllCountryCovidData.Any()? AllCountryCovidData.Select(data => data.report_date)
                                                    .Distinct() 
                                                    .ToList() : new List<DateTime>();
            var lastAddedDate = reportDateList.Any()? reportDateList.Max().Date : new DateTime(2020, 1, 22);
            var todayDate = DateTime.UtcNow.Date;
            var yesterdayDate = todayDate.AddDays(-1).Date;
            var lastWeekDate = todayDate.AddDays(-8).Date;
            var twoWeekBackDate = DateTime.UtcNow.AddDays(-15).Date;

            if (lastAddedDate < yesterdayDate) 
            {
                var startDate = lastAddedDate.AddDays(1);
                var endDate = yesterdayDate;
                var allDates = GetAllDates(startDate, endDate);
                if (allDates.Count > 0)
                {
                    AddCovidData(allDates);
                }
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

        public void AddCovidData(List<DateTime> dateList)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var allCountry = _countryRepository.AllCountryData();
                foreach (var date in dateList)
                {
                    var newCovidData = _covidDataClient.FetchCovidDataAsync(date);
                    
                    if (newCovidData.Any())
                    {
                        var covidData = new List<CovidData>();
                        foreach (var data in newCovidData)
                        {
                            if (data.CountryRegionJH == "Mainland China")
                            {
                                data.CountryRegionJH = "China";
                            }

                            var matchingCountry = allCountry.Where(x => x.country_name == data.CountryRegionJH);
                            covidData.Add(
                                new CovidData()
                                {
                                    country_id = matchingCountry.Any() ? matchingCountry.FirstOrDefault().id : 0,
                                    report_date = date,
                                    db_update_date = DateTime.UtcNow,
                                    confirmed_cases = data.ConfirmedCasesJH,
                                    death_cases = data.DeathCasesJH,
                                    recovered = data.RecoveredJH,
                                    active_cases = data.ActiveCasesJH, //change the code to get Active Cases for old date
                                }
                            );
                        }
                        dbContext.covidcountrydata.AddRange(covidData);
                        dbContext.SaveChanges();
                    }
                }
            }
        }

        public void UpdateCovidData(List<DateTime> dateList)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var allCovidData = dbContext.covidcountrydata.AsEnumerable();
                var allCountry = _countryRepository.AllCountryData();
                foreach (var date in dateList)
                {
                    var updateCovidData = _covidDataClient.FetchCovidDataAsync(date);

                    foreach (var data in updateCovidData)
                    {
                        var singleCountryUpdate = (from covidData in allCovidData
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
    }
}