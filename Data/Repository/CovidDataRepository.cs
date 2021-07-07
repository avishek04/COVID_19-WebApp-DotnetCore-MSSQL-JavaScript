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

        public List<CovidGridDataViewModel> CovidGridData()
        {
            List<CovidGridDataViewModel> covidGridData = new List<CovidGridDataViewModel>();
            try
            {
                UpdateAllCovidData();

                var allCovidData = AllCountryCovidData.ToList();
                var secondLastReportDate = allCovidData.Select(x => x.report_date).Max(date => date).AddDays(-1);
                covidGridData = (from gridData in allCovidData
                                 join country in _countryRepository.AllCountries
                                 on gridData.country_id equals country.id
                                 where gridData.report_date == secondLastReportDate
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
                var secondLastReportDate = allCovidData.Select(x => x.report_date).Max(date => date).AddDays(-1);
                var lastDayRecords = allCovidData.Where(x => x.report_date == secondLastReportDate).ToList();
                var countryGridData = from gridData in lastDayRecords
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
                                      };

                if (lastDayRecords != null)
                {
                    worldGridData.countryName = "World";
                    worldGridData.totalCases = Convert.ToInt64(lastDayRecords.Select(data => data.total_cases).Sum(x => x));
                    worldGridData.totalDeaths = Convert.ToInt64(lastDayRecords.Select(data => data.total_deaths).Sum(x => x));
                    worldGridData.dailyCases = Convert.ToInt64(lastDayRecords.Select(data => data.new_cases).Sum(x => x));
                    worldGridData.dailyDeaths = Convert.ToInt64(lastDayRecords.Select(data => data.new_deaths).Sum(x => x));
                    worldGridData.weeklyCases = Convert.ToInt64(lastDayRecords.Select(data => data.weekly_cases).Sum(x => x));
                    worldGridData.weeklyDeaths = Convert.ToInt64(lastDayRecords.Select(data => data.weekly_deaths).Sum(x => x));
                    worldGridData.biweeklyCases = Convert.ToInt64(lastDayRecords.Select(data => data.biweekly_cases).Sum(x => x));
                    worldGridData.biweeklyDeaths = Convert.ToInt64(lastDayRecords.Select(data => data.biweekly_deaths).Sum(x => x));
                }

                covidGridData.AddRange(countryGridData);
                covidGridData.Add(worldGridData);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return covidGridData;
        }

        //public IEnumerable<CovidGraphDataViewModel> CovidGraphData()
        //{
        //    var covidGraphData = new List<CovidGraphDataViewModel>();
        //    var allCovidData = AllCovidData().Where(x => x.report_date > new DateTime(2020, 2, 1)).ToList();
        //    var lastDayDataSet = allCovidData.Select(x => new { x.report_date, x.country_name, x.confirmed_cases, x.death_cases });

        //    foreach (var covidData in allCovidData)
        //    {
        //        //var dayBefore = covidData.report_date.AddDays(-1);
        //        if (covidData.report_date != null && covidData.country_name != null)
        //        {
        //            var dayBeforeRecord = lastDayDataSet.Where(x => x.report_date == covidData.report_date.AddDays(-1) && x.country_name == covidData.country_name).FirstOrDefault();
        //            if (dayBeforeRecord != null)
        //            {
        //                if (covidData.confirmed_cases != null && covidData.active_cases != null &&
        //                covidData.death_cases != null && dayBeforeRecord.confirmed_cases != null &&
        //                dayBeforeRecord.death_cases != null)
        //                {
        //                    covidGraphData.Add(new CovidGraphDataViewModel
        //                    {
        //                        country_name = covidData.country_name,
        //                        report_date = covidData.report_date,
        //                        new_cases = Convert.ToInt64(covidData.confirmed_cases - dayBeforeRecord.confirmed_cases),
        //                        active_cases = Convert.ToInt64(covidData.active_cases),
        //                        new_deaths = Convert.ToInt64(covidData.death_cases - dayBeforeRecord.death_cases)
        //                    });
        //                }
        //            }
        //        }
        //    }
        //    //covidGraphData.AddRange(covidGraphData);
        //    return covidGraphData;
        //}

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

        //public IEnumerable<CovidGraphDataViewModel> CovidAverageGraphData(string countryUI)
        //{
        //    var covidGraphData = CovidGraphData(countryUI).ToList();

        //    int i = 0;
        //    var covidAvgGraphData = new List<CovidGraphDataViewModel>();
        //    while (i < (covidGraphData.Count() - 2))
        //    {
        //        covidAvgGraphData.Add(new CovidGraphDataViewModel
        //        {
        //            countryName = covidGraphData[i].countryName,
        //            reportDate = covidGraphData[i + 1].reportDate,
        //            totalCases = (covidGraphData[i].totalCases + covidGraphData[i + 1].totalCases + covidGraphData[i + 2].totalCases) / 3,
        //            totalDeaths = (covidGraphData[i].totalDeaths + covidGraphData[i + 1].totalDeaths + covidGraphData[i + 2].totalDeaths) / 3,
        //            newCases = (covidGraphData[i].newCases + covidGraphData[i + 1].newCases + covidGraphData[i + 2].newCases) / 3,
        //            newDeaths = (covidGraphData[i].newDeaths + covidGraphData[i + 1].newDeaths + covidGraphData[i + 2].newDeaths) / 3,
        //        });
        //        i += 3;
        //    }
        //    return covidGraphData;
        //}

        public void UpdateAllCovidData()
        {
            try
            {
                if (!AllCountryCovidData.Any())
                {
                    AddAllCovidData();
                }

                if (AllCountryCovidData.Any())
                {
                    var lastReportDate = AllCountryCovidData.Select(data => data.report_date)
                                                            .Distinct()
                                                            .Max();
                    var dayBeforeYesterdayDate = DateTime.UtcNow.AddDays(-2).Date;
                    if (lastReportDate < dayBeforeYesterdayDate)
                    {
                        AddCovidData(lastReportDate);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void AddAllCovidData()
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
                            if (!matchingCountry.Any())
                            {
                                continue;
                            }
                            var countryId = matchingCountry.FirstOrDefault().id;
                            covidData.Add(
                                new CovidData
                                {
                                    country_id = countryId,
                                    report_date = data.ReportDateJH,
                                    db_update_date = DateTime.UtcNow.Date,
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

        public void AddCovidData(DateTime lastReportDate)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var allCountry = _countryRepository.AllCountryData();
                    var covidApiData = _covidDataClient.FetchCovidDataAsync().Where(data => data.ReportDateJH > lastReportDate);

                    if (covidApiData.Any())
                    {
                        var covidData = new List<CovidData>();
                        foreach (var data in covidApiData)
                        {
                            var matchingCountry = allCountry.Where(x => x.country_name == data.CountryJH);
                            if (!matchingCountry.Any())
                            {
                                continue;
                            }
                            var countryId = matchingCountry.FirstOrDefault().id;
                            covidData.Add(
                                new CovidData
                                {
                                    country_id = countryId,
                                    report_date = data.ReportDateJH,
                                    db_update_date = DateTime.UtcNow.Date,
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