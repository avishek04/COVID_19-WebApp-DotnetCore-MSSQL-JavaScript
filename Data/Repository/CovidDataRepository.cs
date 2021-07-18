using System;
using System.Collections.Generic;
using System.Linq;
using COVID_19.CoreApiClient;
using COVID_19.CoreApiClient.Mappers;
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

        public List<CovidGridDataViewModel> CovidGridData()
        {
            List<CovidGridDataViewModel> covidGridData = new List<CovidGridDataViewModel>();
            try
            {
                //UpdateAllCovidData();

                var allCovidData = AllCountryCovidData.ToList();
                var latestReportDate = allCovidData.Select(x => x.report_date).Max(date => date);
                covidGridData = (from gridData in allCovidData
                                 join country in _countryRepository.AllCountries
                                 on gridData.country_id equals country.id
                                 where gridData.report_date == latestReportDate
                                 select new CovidGridDataViewModel
                                 {
                                     countryName = country.country_name,
                                     totalCases = Convert.ToInt64(gridData.total_cases != null ? gridData.total_cases : 0),
                                     totalDeaths = Convert.ToInt64(gridData.total_deaths != null ? gridData.total_deaths : 0),
                                     dailyCases = Convert.ToInt64(gridData.new_cases != null ? gridData.new_cases : 0),
                                     dailyDeaths = Convert.ToInt64(gridData.new_deaths != null ? gridData.new_deaths : 0),
                                     weeklyCases = Convert.ToInt64(gridData.weekly_cases != null ? gridData.weekly_cases : 0),
                                     weeklyDeaths = Convert.ToInt64(gridData.weekly_deaths != null ? gridData.weekly_deaths : 0),
                                     biweeklyCases = Convert.ToInt64(gridData.biweekly_cases != null ? gridData.biweekly_cases : 0),
                                     biweeklyDeaths = Convert.ToInt64(gridData.biweekly_deaths != null ? gridData.biweekly_deaths : 0)
                                 }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return covidGridData;
        }

        public List<CovidGridDataViewModel> CovidCountryGridData(string countryUI)
        {
            List<CovidGridDataViewModel> covidGridData = new List<CovidGridDataViewModel>();
            CovidGridDataViewModel worldGridData = new CovidGridDataViewModel();
            var countryName = "India";

            try
            {
                if (countryUI != null || countryUI != "")
                {
                    countryName = countryUI;
                }
                UpdateAllCovidData();

                var allCovidData = AllCountryCovidData.ToList();
                var latestReportDate = allCovidData.Select(x => x.report_date).Max(date => date);
                var lastDayRecords = allCovidData.Where(x => x.report_date == latestReportDate).ToList();

                if (lastDayRecords != null)
                {
                    var countryGridData = (from gridData in lastDayRecords
                                          join country in _countryRepository.AllCountries
                                          on gridData.country_id equals country.id
                                          where country.country_name == countryName
                                          select new CovidGridDataViewModel
                                          {
                                              countryName = country.country_name,
                                              totalCases = Convert.ToInt64(gridData.total_cases != null ? gridData.total_cases : 0),
                                              totalDeaths = Convert.ToInt64(gridData.total_deaths != null ? gridData.total_deaths : 0),
                                              dailyCases = Convert.ToInt64(gridData.new_cases != null ? gridData.new_cases : 0),
                                              dailyDeaths = Convert.ToInt64(gridData.new_deaths != null ? gridData.new_deaths : 0),
                                              weeklyCases = Convert.ToInt64(gridData.weekly_cases != null ? gridData.weekly_cases : 0),
                                              weeklyDeaths = Convert.ToInt64(gridData.weekly_deaths != null ? gridData.weekly_deaths : 0),
                                              biweeklyCases = Convert.ToInt64(gridData.biweekly_cases != null ? gridData.biweekly_cases : 0),
                                              biweeklyDeaths = Convert.ToInt64(gridData.biweekly_deaths != null ? gridData.biweekly_deaths : 0)
                                          }).FirstOrDefault();

                    worldGridData.countryName = "World";
                    worldGridData.totalCases = Convert.ToInt64(lastDayRecords.Select(data => data.total_cases).Sum(x => x));
                    worldGridData.totalDeaths = Convert.ToInt64(lastDayRecords.Select(data => data.total_deaths).Sum(x => x));
                    worldGridData.dailyCases = Convert.ToInt64(lastDayRecords.Select(data => data.new_cases).Sum(x => x));
                    worldGridData.dailyDeaths = Convert.ToInt64(lastDayRecords.Select(data => data.new_deaths).Sum(x => x));
                    worldGridData.weeklyCases = Convert.ToInt64(lastDayRecords.Select(data => data.weekly_cases).Sum(x => x));
                    worldGridData.weeklyDeaths = Convert.ToInt64(lastDayRecords.Select(data => data.weekly_deaths).Sum(x => x));
                    worldGridData.biweeklyCases = Convert.ToInt64(lastDayRecords.Select(data => data.biweekly_cases).Sum(x => x));
                    worldGridData.biweeklyDeaths = Convert.ToInt64(lastDayRecords.Select(data => data.biweekly_deaths).Sum(x => x));
                    covidGridData.Add(countryGridData);
                    covidGridData.Add(worldGridData);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return covidGridData;
        }

        public List<CovidGraphDataViewModel> CovidGraphData(string countryUI)
        {
            List<CovidGraphDataViewModel> covidGraphData = new List<CovidGraphDataViewModel>();
            var countryName = "India";
            try
            {
                if (countryUI != null || countryUI != "")
                {
                    countryName = countryUI;
                }

                var allCovidData = AllCountryCovidData.ToList();
                if (allCovidData != null)
                {
                    covidGraphData = (from graphData in allCovidData
                                      join country in _countryRepository.AllCountries
                                      on graphData.country_id equals country.id
                                      where country.country_name == countryName
                                      select new CovidGraphDataViewModel
                                      {
                                          countryName = country.country_name,
                                          reportDate = graphData.report_date.Date != null ? graphData.report_date.Date : DateTime.Now,
                                          totalCases = Convert.ToInt64(graphData.total_cases != null ? graphData.total_cases : 0),
                                          totalDeaths = Convert.ToInt64(graphData.total_deaths != null ? graphData.total_deaths : 0),
                                          dailyCases = Convert.ToInt64(graphData.new_cases != null ? graphData.new_cases : 0),
                                          dailyDeaths = Convert.ToInt64(graphData.new_deaths != null ? graphData.new_deaths : 0)
                                      }).ToList();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
            return covidGraphData;
        }

        private void UpdateAllCovidData()
        {
            try
            {
                var lastUpdateDate = AllCountryCovidData.Select(x => x.db_update_date).Max();

                if (AllCountryCovidData.Any())
                {
                    if (lastUpdateDate < DateTime.Now.AddDays(-1))
                    {
                        UpdateLastWeekData();
                    }
                }
                else
                {
                    AddAllCovidData();
                }
                

                //if (AllCountryCovidData.Any())
                //{
                //    var lastReportDate = AllCountryCovidData.Select(data => data.report_date)
                //                                            .Distinct()
                //                                            .Max();
                //    var dayBeforeYesterdayDate = DateTime.UtcNow.AddDays(-2).Date;
                //    if (lastReportDate < dayBeforeYesterdayDate)
                //    {
                //        AddCovidData(lastReportDate);
                //    }
                //}
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateLastWeekData()
        {
            try
            {
                var startDate = DateTime.Now.AddDays(-8);
                var endDate = DateTime.Now.AddDays(-1);
                List<DateTime> lastWeek = GetAllDates(startDate, endDate);

                foreach (var date in lastWeek)
                {
                    UpdateDateData(date);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateDateData(DateTime date)
        {
            try
            {
                var countryList = _appDbContext.country.Select(x => new { x.id, x.country_name });
                var covidApiDataForDate = _covidDataClient.FetchCovidDataAsync().Where(data => data.ReportDateJH.Date == date);
                var isRecordPresent = AllCountryCovidData.Where(x => x.report_date == date).Any();
                if (isRecordPresent)
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                        var allCountry = _countryRepository.AllCountryData();
                        foreach (var record in covidApiDataForDate)
                        {
                            var countryId = countryList.Where(x => x.country_name == record.CountryJH);
                            if (countryId.Any())
                            {
                                var countryDateRecord = dbContext.covidcountrydata.Where(x => x.report_date == date && x.country_id == countryId.FirstOrDefault().id);
                                if (countryDateRecord.Any())
                                {
                                    var recordUpdating = countryDateRecord.FirstOrDefault();
                                    recordUpdating.db_update_date = DateTime.UtcNow;
                                    recordUpdating.new_cases = record.NewCasesJH != null ? record.NewCasesJH : 0;
                                    recordUpdating.new_deaths = record.NewDeathsJH != null ? record.NewDeathsJH : 0;
                                    recordUpdating.weekly_cases = record.WeeklyCasesJH != null ? record.WeeklyCasesJH : 0;
                                    recordUpdating.weekly_deaths = record.WeeklyDeathsJH != null ? record.WeeklyDeathsJH : 0;
                                    recordUpdating.biweekly_cases = record.BiWeeklyCasesJH != null ? record.BiWeeklyCasesJH : 0;
                                    recordUpdating.biweekly_deaths = record.BiWeeklyDeathsJH != null ? record.BiWeeklyDeathsJH : 0;
                                    recordUpdating.total_cases = record.TotalCasesJH != null ? record.TotalCasesJH : 0;
                                    recordUpdating.total_deaths = record.TotalDeathsJH != null ? record.TotalDeathsJH : 0;
                                }
                            }
                        }
                        dbContext.SaveChanges();
                    }
                }
                else if (covidApiDataForDate != null)
                {
                    AddDateData(date);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AddDateData(DateTime date)
        {
            try
            {
                var countryList = _appDbContext.country.Select(x => new { x.id, x.country_name });
                var covidApiDataForDate = _covidDataClient.FetchCovidDataAsync().Where(data => data.ReportDateJH.Date == date);
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    List<CovidData> dataToAdd = new List<CovidData>();

                    foreach (var record in covidApiDataForDate)
                    {
                        var countryId = countryList.Where(x => x.country_name == record.CountryJH);
                        if (countryId.Any())
                        {
                            CovidData eachCountryData = new CovidData();
                            eachCountryData.country_id = countryId.FirstOrDefault().id;
                            eachCountryData.report_date = date;
                            eachCountryData.db_update_date = DateTime.UtcNow;
                            eachCountryData.new_cases = record.NewCasesJH != null ? record.NewCasesJH : 0;
                            eachCountryData.new_deaths = record.NewDeathsJH != null ? record.NewDeathsJH : 0;
                            eachCountryData.weekly_cases = record.WeeklyCasesJH != null ? record.WeeklyCasesJH : 0;
                            eachCountryData.weekly_deaths = record.WeeklyDeathsJH != null ? record.WeeklyDeathsJH : 0;
                            eachCountryData.biweekly_cases = record.BiWeeklyCasesJH != null ? record.BiWeeklyCasesJH : 0;
                            eachCountryData.biweekly_deaths = record.BiWeeklyDeathsJH != null ? record.BiWeeklyDeathsJH : 0;
                            eachCountryData.total_cases = record.TotalCasesJH != null ? record.TotalCasesJH : 0;
                            eachCountryData.total_deaths = record.TotalDeathsJH != null ? record.TotalDeathsJH : 0;
                            dataToAdd.Add(eachCountryData);
                        }
                    }
                    dbContext.covidcountrydata.AddRange(dataToAdd);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AddAllCovidData()
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var allCountry = _countryRepository.AllCountryData();
                    var covidApiData = _covidDataClient.FetchCovidDataAsync();
                    if (covidApiData.Any())
                    {
                        var covidData = new List<CovidData>();
                        foreach (var data in covidApiData)
                        {
                            var matchingCountry = allCountry.Where(x => x.country_name == data.CountryJH);
                            if (matchingCountry != null)
                            {
                                var countryId = matchingCountry.FirstOrDefault().id;
                                covidData.Add(
                                    new CovidData
                                    {
                                        country_id = countryId,
                                        report_date = data.ReportDateJH,
                                        db_update_date = DateTime.UtcNow,
                                        new_cases = data.NewCasesJH != null ? data.NewCasesJH : 0,
                                        new_deaths = data.NewDeathsJH != null ? data.NewDeathsJH : 0,
                                        weekly_cases = data.WeeklyCasesJH != null ? data.WeeklyCasesJH : 0,
                                        weekly_deaths = data.WeeklyDeathsJH != null ? data.WeeklyDeathsJH : 0,
                                        biweekly_cases = data.BiWeeklyCasesJH != null ? data.BiWeeklyCasesJH : 0,
                                        biweekly_deaths = data.BiWeeklyDeathsJH != null ? data.BiWeeklyDeathsJH : 0,
                                        total_cases = data.TotalCasesJH != null ? data.TotalCasesJH : 0,
                                        total_deaths = data.TotalDeathsJH != null ? data.TotalDeathsJH : 0
                                    }
                                );
                            }
                        }
                        dbContext.covidcountrydata.AddRange(covidData);
                        dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void AddCovidData(DateTime lastReportDate)
        //{
        //    try
        //    {
        //        using (var scope = _serviceScopeFactory.CreateScope())
        //        {
        //            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        //            var allCountry = _countryRepository.AllCountryData();
        //            var covidApiData = _covidDataClient.FetchCovidDataAsync().Where(data => data.ReportDateJH > lastReportDate);

        //            if (covidApiData.Any())
        //            {
        //                var covidData = new List<CovidData>();
        //                foreach (var data in covidApiData)
        //                {
        //                    var matchingCountry = allCountry.Where(x => x.country_name == data.CountryJH);
        //                    if (matchingCountry.Any())
        //                    {
        //                        var countryId = matchingCountry.FirstOrDefault().id;
        //                        covidData.Add(
        //                            new CovidData
        //                            {
        //                                country_id = countryId,
        //                                report_date = data.ReportDateJH,
        //                                db_update_date = DateTime.UtcNow.Date,
        //                                new_cases = data.NewCasesJH != null ? data.NewCasesJH : 0,
        //                                new_deaths = data.NewDeathsJH != null ? data.NewDeathsJH : 0,
        //                                weekly_cases = data.WeeklyCasesJH != null ? data.WeeklyCasesJH : 0,
        //                                weekly_deaths = data.WeeklyDeathsJH != null ? data.WeeklyDeathsJH : 0,
        //                                biweekly_cases = data.BiWeeklyCasesJH != null ? data.BiWeeklyCasesJH : 0,
        //                                biweekly_deaths = data.BiWeeklyDeathsJH != null ? data.BiWeeklyDeathsJH : 0,
        //                                total_cases = data.TotalCasesJH != null ? data.TotalCasesJH : 0,
        //                                total_deaths = data.TotalDeathsJH != null ? data.TotalDeathsJH : 0
        //                            }
        //                        );
        //                    }
        //                }
        //                dbContext.covidcountrydata.AddRange(covidData);
        //                dbContext.SaveChanges();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public List<DateTime> GetAllDates(DateTime startDate, DateTime endDate)
        {
            List<DateTime> allDates = new List<DateTime>();
            try
            {
                var start = startDate.Date;
                var end = endDate.Date;
                for (DateTime i = start; i <= end; i = i.AddDays(1))
                {
                    allDates.Add(i);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return allDates;
        }
    }
}