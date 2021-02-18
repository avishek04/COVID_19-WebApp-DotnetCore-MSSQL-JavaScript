using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COVID_19.CoreApiClient;
using COVID_19.Models;
using Microsoft.Extensions.DependencyInjection;

namespace COVID_19.Data.Repository
{
    public class CovidDataRepository : ICovidDataRepository
    {
        public AppDbContext _appDbContext;
        public ICovidDataClient _covidDataClient;
        public IServiceScopeFactory _serviceScopeFactory;

        public CovidDataRepository(AppDbContext appDbContext, ICovidDataClient covidDataClient, IServiceScopeFactory serviceScopeFactory)
        {
            _appDbContext = appDbContext;
            _covidDataClient = covidDataClient;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IEnumerable<CovidCountryData> AllCountryCovidData
        {
            get
            {
                var allCountryCovidData = _appDbContext.CovidCountryData;
                return allCountryCovidData;
            }
        }

        public IEnumerable<CovidCountryData> GetAllCovidData()
        {
            //if (!allCountryCovidData.Any())
            //{
            //    AddCovidCountryDataAsync();
            //    return _appDbContext.CovidCountryData;
            //}
            //else
            try
            {
                var oneDayBefore = DateTime.UtcNow.AddDays(-1);
                var allCountryCovidData = AllCountryCovidData.ToList();


                if (allCountryCovidData.Where(x => x.Last_Update_Date <= oneDayBefore).Count() > 0 || !(allCountryCovidData.Where(x => x.Report_Date >= oneDayBefore).Count() > 0))
                {
                    UpdateCovidCountryDataAsync(allCountryCovidData);
                    return _appDbContext.CovidCountryData;
                    //return dbContext.CovidCountryData;
                }
                else
                {
                    return _appDbContext.CovidCountryData;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<DateTime> GetAllDates(DateTime startingDate, DateTime endingDate)
        {
            List<DateTime> allDates = new List<DateTime>();
            var startDate = startingDate.Date;
            var endDate = endingDate.Date;
            for (DateTime i = startDate; i <= endDate; i = i.AddDays(1))
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

        public async void UpdateCovidCountryDataAsync(List<CovidCountryData> countryCovidData)
        {
            try
            {
                var startDate = new DateTime(2020, 1, 22);
                var todaysDate = DateTime.UtcNow.Date;
                var oneDayBefore = DateTime.UtcNow.AddDays(-1);
                var pastRecordsToUpdate = countryCovidData.Where(x => x.Last_Update_Date <= oneDayBefore);
                var datesToUpdate = pastRecordsToUpdate.Select(x => x.Report_Date).Distinct().ToArray();

                if (pastRecordsToUpdate.Count() > 0)
                {
                    foreach (var date in datesToUpdate)
                    {
                        var path = $"https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/${date}.csv";
                        var newRecord = await _covidDataClient.FetchCovidCountryDataAsync(path);

                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                            var recordsToUpdate = dbContext.CovidCountryData.Where(x => x.Report_Date == date);
                            foreach (var record in recordsToUpdate)
                            {
                                var newData = newRecord.Where(x => x.Report_Date == record.Report_Date && x.Combined_Key == record.Combined_Key).FirstOrDefault();
                                record.ConfirmedCases = newData.ConfirmedCases;
                                record.Deaths = newData.Deaths;
                                record.Recovered = newData.Recovered;
                                record.ActiveCases = newData.ActiveCases;
                            }
                            dbContext.SaveChanges();
                        }
                    }
                }
                var reportDateInDb = countryCovidData.Select(x => x.Report_Date).Distinct().ToArray();
                var dateToAdd = new List<DateTime>();
                for (var i = startDate; i <= todaysDate; i = i.AddDays(1))
                {
                    if (!reportDateInDb.Contains(i))
                    {
                        dateToAdd.Add(i);
                    }
                }
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    //var todaysRecordsDownloaded = dbContext.CovidCountryData.Where(x => x.Report_Date > oneDayBefore);
                    //if (!todaysRecordsDownloaded.Any())
                    //{
                    //    var path = $"https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/${todaysDate}.csv";
                    //    var newRecord = await _covidDataClient.FetchCovidCountryDataAsync(path);
                    //    dbContext.CovidCountryData.AddRange(newRecord);
                    //}
                    foreach (var date in dateToAdd)
                    {
                        var path = $"https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/${date.Date}.csv";
                        var newRecord = await _covidDataClient.FetchCovidCountryDataAsync(path);
                        dbContext.CovidCountryData.AddRange(newRecord);
                    }
                    dbContext.SaveChanges();
                    //var dataFromCsv = await _covidDataClient.FetchCovidCountryDataAsync();
                    //foreach (var data in dataFromCsv)
                    //{
                    //    data.Last_Update_Date = DateTime.UtcNow;
                    //}

                    //using (var scope = serviceScopeFactory.CreateScope())
                    //{
                    //    var dbContext = scope.ServiceProvider.GetService<AppDbContext>();
                    //    var covidCountryDataDb = dbContext.CovidCountryData;
                    //    var covidDataToAdd = dataFromCsv.Where(x => covidCountryDataDb.All(y => x.Report_Date != y.Report_Date && x.Combined_Key != y.Combined_Key));
                    //    dbContext.CovidCountryData.AddRange(covidDataToAdd);
                    //    dbContext.SaveChanges();
                    //}

                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}