using System;
using System.Collections.Generic;
using COVID_19.CoreApiClient;
using Microsoft.Extensions.DependencyInjection;
using COVID_19.Models;
using System.Linq;
using COVID_19.Models.ViewModels;
using COVID_19.CoreApiClient.Mappers;

namespace COVID_19.Data.Repository
{
    public class CovidVaccineRepository : ICovidVaccineRepository
    {
        private AppDbContext _appDbContext;
        private ICovidDataClient _covidDataClient;
        private ICountryRepository _countryRepository;
        private IServiceScopeFactory _serviceScopeFactory;

        public CovidVaccineRepository(AppDbContext appDbContext, ICovidDataClient covidDataClient, ICountryRepository countryRepository, IServiceScopeFactory serviceScopeFactory)
        {
            _appDbContext = appDbContext;
            _covidDataClient = covidDataClient;
            _countryRepository = countryRepository;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IEnumerable<CovidVaccineData> AllCovidVaccineData
        {
            get
            {
                return _appDbContext.covidVaccineData;
            }
        }

        public List<VaccineGridDataViewModel> VaccineGridData()
        {
            List<VaccineGridDataViewModel> vaccineGridData = new List<VaccineGridDataViewModel>();
            try
            {
                UpdateAllVaccineData();

                var allCovidVaccineData = AllCovidVaccineData.ToList();
                var latestReportDate = AllCovidVaccineData.Select(x => x.vaccine_report_date).Max(date => date);

                if (latestReportDate != null)
                {
                    vaccineGridData = (from gridData in allCovidVaccineData
                                       join country in _countryRepository.AllCountries
                                       on gridData.country_id equals country.id
                                       where gridData.vaccine_report_date == latestReportDate
                                       select new VaccineGridDataViewModel
                                       {
                                           countryName = country.country_name,
                                           dailyVaccinations = Convert.ToInt64(gridData.daily_vaccinations != null ? gridData.daily_vaccinations : 0),
                                           dailyVaccinationsPerMillion = Convert.ToInt64(gridData.daily_vaccinations_per_million != null ? gridData.daily_vaccinations_per_million : 0),
                                           peopleVaccinated = Convert.ToInt64(gridData.people_vaccinated != null ? gridData.people_vaccinated : 0),
                                           peopleFullyVaccinatedPerHun = Convert.ToInt64(gridData.people_fully_vaccinated_per_hundred != null ? gridData.people_fully_vaccinated_per_hundred : 0),
                                           peopleFullyVaccinated = Convert.ToInt64(gridData.people_fully_vaccinated != null ? gridData.people_fully_vaccinated : 0),
                                           peopleVaccinatedPerHun = Convert.ToInt64(gridData.people_vaccinated_per_hundred != null ? gridData.people_vaccinated_per_hundred : 0),
                                           totalVaccinations = Convert.ToInt64(gridData.total_vaccinations != null ? gridData.total_vaccinations : 0),
                                           totalVaccinationPerHun = Convert.ToInt64(gridData.total_vaccinations_per_hundred != null ? gridData.total_vaccinations_per_hundred : 0)
                                       }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return vaccineGridData;
        }

        public List<VaccineGridDataViewModel> VaccineCountryGridData(string countryUI)
        {
            List<VaccineGridDataViewModel> vaccineGridData = new List<VaccineGridDataViewModel>();
            VaccineGridDataViewModel worldGridData = new VaccineGridDataViewModel();
            var countryName = "India";

            try
            {
                if (countryUI != null || countryUI != "")
                {
                    countryName = countryUI;
                }
                UpdateAllVaccineData();

                var allCovidVaccineData = AllCovidVaccineData.ToList();
                var latestReportDate = allCovidVaccineData.Select(x => x.vaccine_report_date).Max(date => date);
                var lastDayRecords = allCovidVaccineData.Where(x => x.vaccine_report_date == latestReportDate).ToList();

                if (lastDayRecords != null) {
                    var recordCount = lastDayRecords.Count();

                    var countryGridData = (from gridData in lastDayRecords
                                          join country in _countryRepository.AllCountries
                                          on gridData.country_id equals country.id
                                          where country.country_name == countryName
                                          select new VaccineGridDataViewModel
                                          {
                                              countryName = country.country_name,
                                              dailyVaccinations = Convert.ToInt64(gridData.daily_vaccinations != null ? gridData.daily_vaccinations : 0),
                                              dailyVaccinationsPerMillion = Convert.ToInt64(gridData.daily_vaccinations_per_million != null ? gridData.daily_vaccinations_per_million : 0),
                                              peopleVaccinated = Convert.ToInt64(gridData.people_vaccinated != null ? gridData.people_vaccinated : 0),
                                              peopleFullyVaccinatedPerHun = Convert.ToInt64(gridData.people_fully_vaccinated_per_hundred != null ? gridData.people_fully_vaccinated_per_hundred : 0),
                                              peopleFullyVaccinated = Convert.ToInt64(gridData.people_fully_vaccinated != null ? gridData.people_fully_vaccinated : 0),
                                              peopleVaccinatedPerHun = Convert.ToInt64(gridData.people_vaccinated_per_hundred != null ? gridData.people_vaccinated_per_hundred : 0),
                                              totalVaccinations = Convert.ToInt64(gridData.total_vaccinations != null ? gridData.total_vaccinations : 0),
                                              totalVaccinationPerHun = Convert.ToInt64(gridData.total_vaccinations_per_hundred != null ? gridData.total_vaccinations_per_hundred : 0)
                                          }).FirstOrDefault();
                    worldGridData.countryName = "World";
                    worldGridData.dailyVaccinations = Convert.ToInt64(lastDayRecords.Select(data => data.daily_vaccinations).Sum(x => x));
                    worldGridData.dailyVaccinationsPerMillion = Convert.ToInt64(lastDayRecords.Select(data => data.daily_vaccinations_per_million).Sum(x => x) / recordCount);
                    worldGridData.peopleVaccinated = Convert.ToInt64(lastDayRecords.Select(data => data.people_vaccinated).Sum(x => x) / recordCount);
                    worldGridData.peopleFullyVaccinatedPerHun = Convert.ToInt64((lastDayRecords.Select(data => data.people_fully_vaccinated_per_hundred).Sum(x => x)) / recordCount);
                    worldGridData.peopleFullyVaccinated = Convert.ToInt64(lastDayRecords.Select(data => data.people_fully_vaccinated).Sum(x => x));
                    worldGridData.peopleVaccinatedPerHun = Convert.ToInt64(lastDayRecords.Select(data => data.people_vaccinated_per_hundred).Sum(x => x) / recordCount);
                    worldGridData.totalVaccinations = Convert.ToInt64(lastDayRecords.Select(data => data.total_vaccinations).Sum(x => x));
                    worldGridData.totalVaccinationPerHun = Convert.ToInt64((lastDayRecords.Select(data => data.total_vaccinations_per_hundred).Sum(x => x)) / recordCount);
                    vaccineGridData.Add(countryGridData);
                    vaccineGridData.Add(worldGridData);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return vaccineGridData;
        }

        public List<VaccineGraphDataViewModel> VaccineGraphData(string countryUI)
        {
            List<VaccineGraphDataViewModel> vaccineGraphData = new List<VaccineGraphDataViewModel>();
            var countryName = "India";
            try
            {
                if (countryUI != null || countryUI != "")
                {
                    countryName = countryUI;
                }

                var allCovidData = AllCovidVaccineData.ToList();
                if (allCovidData != null)
                {
                    vaccineGraphData = (from graphData in allCovidData
                                        join country in _countryRepository.AllCountries
                                        on graphData.country_id equals country.id
                                        where country.country_name == countryName
                                        select new VaccineGraphDataViewModel
                                        {
                                            countryName = country.country_name,
                                            reportDate = graphData.vaccine_report_date != null ? graphData.vaccine_report_date.Date : DateTime.Now,
                                            dailyVaccinations = Convert.ToInt64(graphData.daily_vaccinations != null ? graphData.daily_vaccinations : 0),
                                            peopleVaccinated = Convert.ToInt64(graphData.people_vaccinated != null ? graphData.people_vaccinated : 0),
                                            peopleFullyVaccinated = Convert.ToInt64(graphData.people_fully_vaccinated != null ? graphData.people_fully_vaccinated : 0),
                                            totalVaccinations = Convert.ToInt64(graphData.total_vaccinations != null ? graphData.total_vaccinations : 0)
                                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return vaccineGraphData;
        }

        private void UpdateAllVaccineData()
        {
            try
            {
                var lastUpdateDate = AllCovidVaccineData.Select(x => x.db_update_date_vaccine).Max();

                if (AllCovidVaccineData.Any())
                {
                    if (lastUpdateDate < DateTime.Now.AddDays(-1))
                    {
                        UpdateLastWeekVaccineData();
                    }
                }
                else
                {
                    AddAllVaccineData();
                }

                //if (!AllCovidVaccineData.Any())
                //{
                //    AddAllVaccineData();
                //}

                //if (AllCovidVaccineData.Any())
                //{
                //    var lastReportDate = AllCovidVaccineData.Select(data => data.vaccine_report_date)
                //                                            .Distinct()
                //                                            .Max();
                //    var dayBeforeYesterdayDate = DateTime.UtcNow.AddDays(-2).Date;
                //    if (lastReportDate < dayBeforeYesterdayDate)
                //    {
                //        AddVaccineData(lastReportDate);
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateLastWeekVaccineData()
        {
            try
            {
                var startDate = DateTime.Now.AddDays(-8);
                var endDate = DateTime.Now.AddDays(-1);
                List<DateTime> lastWeek = GetAllDates(startDate, endDate);

                foreach (var date in lastWeek)
                {
                    UpdateVaccineDateData(date);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateVaccineDateData(DateTime date)
        {
            try
            {
                var countryList = _appDbContext.country.Select(x => new { x.id, x.country_name });
                var covidApiDataForDate = _covidDataClient.FetchVaccineDataAsync().Where(data => data.VaccineReportDateJH.Date == date);
                var isRecordPresent = AllCovidVaccineData.Where(x => x.vaccine_report_date == date).Any();
                if (isRecordPresent)
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                        var allCountry = _countryRepository.AllCountryData();
                        foreach (var record in covidApiDataForDate)
                        {
                            var countryId = countryList.Where(x => x.country_name == record.VaccineCountryJH);
                            if (countryId.Any())
                            {
                                var countryDateRecord = dbContext.covidVaccineData.Where(x => x.vaccine_report_date == date && x.country_id == countryId.FirstOrDefault().id);
                                if (countryDateRecord.Any())
                                {
                                    var recordUpdating = countryDateRecord.FirstOrDefault();
                                    recordUpdating.db_update_date_vaccine = DateTime.UtcNow;
                                    recordUpdating.daily_vaccinations = record.DailyVaccinationsJH != null ? record.DailyVaccinationsJH : 0;
                                    recordUpdating.daily_vaccinations_per_million = record.DailyVaccinationsPerMillionJH != null ? record.DailyVaccinationsPerMillionJH : 0;
                                    recordUpdating.people_vaccinated = record.PeopleVaccinatedJH != null ? record.PeopleVaccinatedJH : 0;
                                    recordUpdating.people_vaccinated_per_hundred = record.PeopleVaccinatedPerHundredJH != null ? record.PeopleVaccinatedPerHundredJH : 0;
                                    recordUpdating.people_fully_vaccinated = record.PeopleFullyVaccinatedJH != null ? record.PeopleFullyVaccinatedJH : 0;
                                    recordUpdating.people_fully_vaccinated_per_hundred = record.PeopleFullyVaccinatedPerHundredJH != null ? record.PeopleFullyVaccinatedPerHundredJH : 0;
                                    recordUpdating.total_vaccinations = record.TotalVaccinationsJH != null ? record.TotalVaccinationsJH : 0;
                                    recordUpdating.total_vaccinations_per_hundred = record.TotalVaccinationsPerHundredJH != null ? record.TotalVaccinationsPerHundredJH : 0;
                                }
                                
                            }
                        }
                        dbContext.SaveChanges();
                    }
                }
                else if (covidApiDataForDate != null)
                {
                    AddVaccineDateData(date);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AddVaccineDateData(DateTime date)
        {
            try
            {
                var countryList = _appDbContext.country.Select(x => new { x.id, x.country_name });
                var covidApiDataForDate = _covidDataClient.FetchVaccineDataAsync().Where(data => data.VaccineReportDateJH.Date == date);
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    List<CovidVaccineData> dataToAdd = new List<CovidVaccineData>();

                    foreach (var record in covidApiDataForDate)
                    {
                        var countryId = countryList.Where(x => x.country_name == record.VaccineCountryJH);
                        if (countryId.Any())
                        {
                            CovidVaccineData eachCountryData = new CovidVaccineData();
                            eachCountryData.country_id = countryId.FirstOrDefault().id;
                            eachCountryData.vaccine_report_date = date;
                            eachCountryData.db_update_date_vaccine = DateTime.UtcNow;
                            eachCountryData.daily_vaccinations = record.DailyVaccinationsJH != null ? record.DailyVaccinationsJH : 0;
                            eachCountryData.daily_vaccinations_per_million = record.DailyVaccinationsPerMillionJH != null ? record.DailyVaccinationsPerMillionJH : 0;
                            eachCountryData.people_vaccinated = record.PeopleVaccinatedJH != null ? record.PeopleVaccinatedJH : 0;
                            eachCountryData.people_vaccinated_per_hundred = record.PeopleVaccinatedPerHundredJH != null ? record.PeopleVaccinatedPerHundredJH : 0;
                            eachCountryData.people_fully_vaccinated = record.PeopleFullyVaccinatedJH != null ? record.PeopleFullyVaccinatedJH : 0;
                            eachCountryData.people_fully_vaccinated_per_hundred = record.PeopleFullyVaccinatedPerHundredJH != null ? record.PeopleFullyVaccinatedPerHundredJH : 0;
                            eachCountryData.total_vaccinations = record.TotalVaccinationsJH != null ? record.TotalVaccinationsJH : 0;
                            eachCountryData.total_vaccinations_per_hundred = record.TotalVaccinationsPerHundredJH != null ? record.TotalVaccinationsPerHundredJH : 0;
                            dataToAdd.Add(eachCountryData);
                        }
                    }
                    dbContext.covidVaccineData.AddRange(dataToAdd);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AddAllVaccineData()
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var allCountry = _countryRepository.AllCountryData();
                    var vaccineApiData = _covidDataClient.FetchVaccineDataAsync();
                    if (vaccineApiData.Any())
                    {
                        var vaccineData = new List<CovidVaccineData>();
                        foreach (var data in vaccineApiData)
                        {
                            var matchingCountry = allCountry.Where(x => x.country_name == data.VaccineCountryJH);
                            if (matchingCountry != null)
                            {
                                var countryId = matchingCountry.FirstOrDefault().id;
                                vaccineData.Add(
                                    new CovidVaccineData
                                    {
                                        country_id = countryId,
                                        vaccine_report_date = data.VaccineReportDateJH,
                                        db_update_date_vaccine = DateTime.UtcNow,
                                        daily_vaccinations = data.DailyVaccinationsJH != null ? data.DailyVaccinationsJH : 0,
                                        daily_vaccinations_per_million = data.DailyVaccinationsPerMillionJH != null ? data.DailyVaccinationsPerMillionJH : 0,
                                        people_vaccinated = data.PeopleVaccinatedJH != null ? data.PeopleVaccinatedJH : 0,
                                        people_vaccinated_per_hundred = data.PeopleVaccinatedPerHundredJH != null ? data.PeopleVaccinatedPerHundredJH : 0,
                                        people_fully_vaccinated = data.PeopleFullyVaccinatedJH != null ? data.PeopleFullyVaccinatedJH : 0,
                                        people_fully_vaccinated_per_hundred = data.PeopleFullyVaccinatedPerHundredJH != null ? data.PeopleFullyVaccinatedPerHundredJH : 0,
                                        total_vaccinations = data.TotalVaccinationsJH != null ? data.TotalVaccinationsJH : 0,
                                        total_vaccinations_per_hundred = data.TotalVaccinationsPerHundredJH != null ? data.TotalVaccinationsPerHundredJH : 0
                                    }
                                );
                            }
                        }
                        dbContext.covidVaccineData.AddRange(vaccineData);
                        dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        //public void AddVaccineData(DateTime lastReportDate)
        //{
        //    try
        //    {
        //        using (var scope = _serviceScopeFactory.CreateScope())
        //        {
        //            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        //            var allCountry = _countryRepository.AllCountryData();
        //            var vaccineApiData = _covidDataClient.FetchVaccineDataAsync().Where(data => data.VaccineReportDateJH > lastReportDate);

        //            if (vaccineApiData.Any())
        //            {
        //                var vaccineData = new List<CovidVaccineData>();
        //                foreach (var data in vaccineApiData)
        //                {
        //                    var matchingCountry = allCountry.Where(x => x.country_name == data.VaccineCountryJH);
        //                    if (!matchingCountry.Any())
        //                    {
        //                        continue;
        //                    }
        //                    var countryId = matchingCountry.FirstOrDefault().id;
        //                    vaccineData.Add(
        //                        new CovidVaccineData
        //                        {
        //                            country_id = countryId,
        //                            vaccine_report_date = data.VaccineReportDateJH,
        //                            db_update_date_vaccine = DateTime.UtcNow.Date,
        //                            daily_vaccinations = data.DailyVaccinationsJH != null ? data.DailyVaccinationsJH : 0,
        //                            daily_vaccinations_per_million = data.DailyVaccinationsPerMillionJH != null ? data.DailyVaccinationsPerMillionJH : 0,
        //                            people_vaccinated = data.PeopleVaccinatedJH != null ? data.PeopleVaccinatedJH : 0,
        //                            people_vaccinated_per_hundred = data.PeopleVaccinatedPerHundredJH != null ? data.PeopleVaccinatedPerHundredJH : 0,
        //                            people_fully_vaccinated = data.PeopleFullyVaccinatedJH != null ? data.PeopleFullyVaccinatedJH : 0,
        //                            people_fully_vaccinated_per_hundred = data.PeopleFullyVaccinatedPerHundredJH != null ? data.PeopleFullyVaccinatedPerHundredJH : 0,
        //                            total_vaccinations = data.TotalVaccinationsJH != null ? data.TotalVaccinationsJH : 0,
        //                            total_vaccinations_per_hundred = data.TotalVaccinationsPerHundredJH != null ? data.TotalVaccinationsPerHundredJH : 0
        //                        });
        //                }
        //                dbContext.covidVaccineData.AddRange(vaccineData);
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
