using System;
using System.Collections.Generic;
using COVID_19.CoreApiClient;
using Microsoft.Extensions.DependencyInjection;
using COVID_19.Models;
using System.Linq;
using COVID_19.Models.ViewModels;

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
                var secondLastReportDate = allCovidVaccineData.Select(x => x.vaccine_report_date).Max(date => date).AddDays(-1);

                if (secondLastReportDate != null)
                {
                    vaccineGridData = (from gridData in allCovidVaccineData
                                       join country in _countryRepository.AllCountries
                                       on gridData.country_id equals country.id
                                       where gridData.vaccine_report_date == secondLastReportDate
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
            var vaccineGridData = new List<VaccineGridDataViewModel>();
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
                var lastDayRecords = latestReportDate != null ? allCovidVaccineData.Where(x => x.vaccine_report_date == latestReportDate).ToList() : allCovidVaccineData.Where(x => x.vaccine_report_date == DateTime.Now.AddDays(-2)).ToList();
                var recordCount = lastDayRecords.Count();

                var countryGridData = from gridData in lastDayRecords
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
                                      };

                var worldGridData = new VaccineGridDataViewModel
                {
                    countryName = "World",
                    dailyVaccinations = Convert.ToInt64(lastDayRecords.Select(data => data.daily_vaccinations).Sum(x => x)),
                    dailyVaccinationsPerMillion = Convert.ToInt64((lastDayRecords.Select(data => data.daily_vaccinations_per_million).Sum(x => x))/ recordCount),
                    peopleVaccinated = Convert.ToInt64(lastDayRecords.Select(data => data.people_vaccinated).Sum(x => x)/ recordCount),
                    peopleFullyVaccinatedPerHun = Convert.ToInt64((lastDayRecords.Select(data => data.people_fully_vaccinated_per_hundred).Sum(x => x))/ recordCount),
                    peopleFullyVaccinated = Convert.ToInt64(lastDayRecords.Select(data => data.people_fully_vaccinated).Sum(x => x)),
                    peopleVaccinatedPerHun = Convert.ToInt64((lastDayRecords.Select(data => data.people_vaccinated_per_hundred).Sum(x => x))/recordCount),
                    totalVaccinations = Convert.ToInt64(lastDayRecords.Select(data => data.total_vaccinations).Sum(x => x)),
                    totalVaccinationPerHun = Convert.ToInt64((lastDayRecords.Select(data => data.total_vaccinations_per_hundred).Sum(x => x))/recordCount)
                };
                vaccineGridData.AddRange(countryGridData);
                vaccineGridData.Add(worldGridData);
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
                vaccineGraphData = (from graphData in allCovidData
                                    join country in _countryRepository.AllCountries
                                    on graphData.country_id equals country.id
                                    where country.country_name == countryName
                                    select new VaccineGraphDataViewModel
                                    {
                                        countryName = country.country_name,
                                        reportDate = graphData.vaccine_report_date != null ? graphData.vaccine_report_date.Date : DateTime.Now,
                                        dailyVaccinations = Convert.ToInt64(graphData.daily_vaccinations),
                                        peopleVaccinated = Convert.ToInt64(graphData.people_vaccinated),
                                        peopleFullyVaccinated = Convert.ToInt64(graphData.people_fully_vaccinated),
                                        totalVaccinations = Convert.ToInt64(graphData.total_vaccinations)
                                    }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return vaccineGraphData;
        }

        public void UpdateAllVaccineData()
        {
            try
            {
                if (!AllCovidVaccineData.Any())
                {
                    AddAllVaccineData();
                }

                if (AllCovidVaccineData.Any())
                {
                    var lastReportDate = AllCovidVaccineData.Select(data => data.vaccine_report_date)
                                                            .Distinct()
                                                            .Max();
                    var dayBeforeYesterdayDate = DateTime.UtcNow.AddDays(-2).Date;
                    if (lastReportDate < dayBeforeYesterdayDate)
                    {
                        AddVaccineData(lastReportDate);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddAllVaccineData()
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
                            if (!matchingCountry.Any())
                            {
                                continue;
                            }
                            var countryId = matchingCountry.FirstOrDefault().id;
                            vaccineData.Add(
                                new CovidVaccineData
                                {
                                    country_id = countryId,
                                    vaccine_report_date = data.VaccineReportDateJH,
                                    db_update_date_vaccine = DateTime.UtcNow.Date,
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

        public void AddVaccineData(DateTime lastReportDate)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var allCountry = _countryRepository.AllCountryData();
                    var vaccineApiData = _covidDataClient.FetchVaccineDataAsync().Where(data => data.VaccineReportDateJH > lastReportDate);

                    if (vaccineApiData.Any())
                    {
                        var vaccineData = new List<CovidVaccineData>();
                        foreach (var data in vaccineApiData)
                        {
                            var matchingCountry = allCountry.Where(x => x.country_name == data.VaccineCountryJH);
                            if (!matchingCountry.Any())
                            {
                                continue;
                            }
                            var countryId = matchingCountry.FirstOrDefault().id;
                            vaccineData.Add(
                                new CovidVaccineData
                                {
                                    country_id = countryId,
                                    vaccine_report_date = data.VaccineReportDateJH,
                                    db_update_date_vaccine = DateTime.UtcNow.Date,
                                    daily_vaccinations = data.DailyVaccinationsJH != null ? data.DailyVaccinationsJH : 0,
                                    daily_vaccinations_per_million = data.DailyVaccinationsPerMillionJH != null ? data.DailyVaccinationsPerMillionJH : 0,
                                    people_vaccinated = data.PeopleVaccinatedJH != null ? data.PeopleVaccinatedJH : 0,
                                    people_vaccinated_per_hundred = data.PeopleVaccinatedPerHundredJH != null ? data.PeopleVaccinatedPerHundredJH : 0,
                                    people_fully_vaccinated = data.PeopleFullyVaccinatedJH != null ? data.PeopleFullyVaccinatedJH : 0,
                                    people_fully_vaccinated_per_hundred = data.PeopleFullyVaccinatedPerHundredJH != null ? data.PeopleFullyVaccinatedPerHundredJH : 0,
                                    total_vaccinations = data.TotalVaccinationsJH != null ? data.TotalVaccinationsJH : 0,
                                    total_vaccinations_per_hundred = data.TotalVaccinationsPerHundredJH != null ? data.TotalVaccinationsPerHundredJH : 0
                                });
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
