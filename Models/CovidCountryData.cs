using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace COVID_19.Models
{
    public class CovidCountryData
    {
        [Key]
        public int Id { get; set; }

        [Name("Province_State")]
        public string Province_State { get; set; }

        [Name("Country_Region")]
        public string Country_Region { get; set; }

        [DataType(DataType.DateTime)]
        [Name("Last_Update")]
        public DateTime Last_Update { get; set; }

        [Column(TypeName = "decimal(6,3)")]
        [Name("Lat")]
        public decimal? Latitude { get; set; }

        [Column(TypeName = "decimal(6,3)")]
        [Name("Long_")]
        public decimal? Longitude { get; set; }

        [Name("Confirmed")]
        public int? ConfirmedCases { get; set; }

        [Name("Deaths")]
        public int? Deaths { get; set; }

        [Name("Recovered")]
        public int? Recovered { get; set; }

        [Name("Active")]
        public int? ActiveCases { get; set; }

        [Name("Combined_Key")]
        public string Combined_Key { get; set; }

        [Column(TypeName = "decimal(6,3)")]
        [Name("Incident_Rate")]
        public decimal? Incident_Rate { get; set; }

        [Column(TypeName = "decimal(6,3)")]
        [Name("Case_Fatality_Ratio")]
        public decimal? Case_Fatality_Ratio { get; set; }
        //FIPS,Admin2,Province_State,Country_Region,Last_Update,Lat,Long_,Confirmed,Deaths,Recovered,Active,Combined_Key,Incident_Rate,Case_Fatality_Ratio
    }
}