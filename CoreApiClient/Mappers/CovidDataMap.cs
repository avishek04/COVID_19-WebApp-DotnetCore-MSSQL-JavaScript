using System;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace COVID_19.CoreApiClient.Mappers
{
    public class CovidDataMap : ClassMap<CovidDataModel>
    {
        public CovidDataMap()
        {
            Map(m => m.CountryJH).Name("location");
            Map(m => m.ReportDateJH).Name("date");
            Map(m => m.NewCasesJH).Name("new_cases");
            Map(m => m.NewDeathsJH).Name("new_deaths");
            Map(m => m.WeeklyCasesJH).Name("weekly_cases");
            Map(m => m.WeeklyDeathsJH).Name("weekly_deaths");
            Map(m => m.BiWeeklyCasesJH).Name("biweekly_cases");
            Map(m => m.BiWeeklyDeathsJH).Name("biweekly_deaths");
            Map(m => m.TotalCasesJH).Name("total_cases"); 
            Map(m => m.TotalDeathsJH).Name("total_deaths"); 
        }
    }

    public class VaccineDataMap : ClassMap<VaccineDataModel>
    {
        public VaccineDataMap()
        {
            Map(m => m.VaccineCountryJH).Name("location");
            Map(m => m.VaccineReportDateJH).Name("date");
            Map(m => m.DailyVaccinationsJH).Name("daily_vaccinations");
            Map(m => m.DailyVaccinationsPerMillionJH).Name("daily_vaccinations_per_million");
            Map(m => m.PeopleVaccinatedJH).Name("people_vaccinated");
            Map(m => m.PeopleVaccinatedPerHundredJH).Name("people_vaccinated_per_hundred");
            Map(m => m.PeopleFullyVaccinatedJH).Name("people_fully_vaccinated");
            Map(m => m.PeopleFullyVaccinatedPerHundredJH).Name("people_fully_vaccinated_per_hundred");
            Map(m => m.TotalVaccinationsJH).Name("total_vaccinations");
            Map(m => m.TotalVaccinationsPerHundredJH).Name("total_vaccinations_per_hundred");        }
    }

    //public class CovidOldDataMap : ClassMap<CovidDataModel>
    //{
    //    public CovidOldDataMap()
    //    {
    //        Map(m => m.CountryRegionJH).Name("Country/Region").Optional();
    //        Map(m => m.ConfirmedCasesJH).Name("Confirmed");
    //        Map(m => m.DeathCasesJH).Name("Deaths");
    //        Map(m => m.RecoveredJH).Name("Recovered");
    //        //Map(m => m.ActiveCasesJH).Name("Active").Optional();
    //    }
    //}

    public class CovidDataModel
    {
        public string CountryJH { get; set; }

        public DateTime ReportDateJH { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal? NewCasesJH { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? NewDeathsJH { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? WeeklyCasesJH { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? WeeklyDeathsJH { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? BiWeeklyCasesJH { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? BiWeeklyDeathsJH { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? TotalCasesJH { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? TotalDeathsJH { get; set; }
    }

    public class VaccineDataModel
    {
        public string VaccineCountryJH { get; set; }

        public DateTime VaccineReportDateJH { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? DailyVaccinationsJH { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? DailyVaccinationsPerMillionJH { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? PeopleVaccinatedJH { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? PeopleVaccinatedPerHundredJH { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? PeopleFullyVaccinatedJH { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? PeopleFullyVaccinatedPerHundredJH { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? TotalVaccinationsJH { get; set; }
        
        [Column(TypeName = "decimal(20,5)")]
        public decimal? TotalVaccinationsPerHundredJH { get; set; }
    }
}