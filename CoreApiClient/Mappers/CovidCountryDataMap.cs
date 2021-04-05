using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace COVID_19.CoreApiClient.Mappers
{
    public class CovidDataMap : ClassMap<CovidDataModel>
    {
        public CovidDataMap()
        {
            Map(m => m.CountryRegionJH).Name("Country_Region").Optional();
            Map(m => m.ConfirmedCasesJH).Name("Confirmed");
            Map(m => m.DeathCasesJH).Name("Deaths");
            Map(m => m.RecoveredJH).Name("Recovered");
            Map(m => m.ActiveCasesJH).Name("Active").Optional();
        }
    }

    public class CovidOldDataMap : ClassMap<CovidDataModel>
    {
        public CovidOldDataMap()
        {
            Map(m => m.CountryRegionJH).Name("Country/Region").Optional();
            Map(m => m.ConfirmedCasesJH).Name("Confirmed");
            Map(m => m.DeathCasesJH).Name("Deaths");
            Map(m => m.RecoveredJH).Name("Recovered");
            //Map(m => m.ActiveCasesJH).Name("Active").Optional();
        }
    }

    public class CovidDataModel
    {
        public string CountryRegionJH { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal? ConfirmedCasesJH { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? DeathCasesJH { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? RecoveredJH { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? ActiveCasesJH { get; set; }
    }
}