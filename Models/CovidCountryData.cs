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
        public DateTime Report_Date { get; set; }

        [Column(TypeName = "decimal(20, 10)")]
        [Name("Lat")]
        public decimal? Latitude { get; set; }

        [Column(TypeName = "decimal(20,10)")]
        [Name("Long_")]
        public decimal? Longitude { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        [Name("Confirmed")]
        public decimal? ConfirmedCases { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        [Name("Deaths")]
        public decimal? Deaths { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        [Name("Recovered")]
        public decimal? Recovered { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        [Name("Active")]
        public decimal? ActiveCases { get; set; }

        [Name("Combined_Key")]
        public string Combined_Key { get; set; }

        [Column(TypeName = "decimal(20,10)")]
        [Name("Incident_Rate")]
        public decimal? Incident_Rate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Last_Update_Date { get; set; }
        //[Column(TypeName = "decimal(6,3)")]
        //[Name("Case_Fatality_Ratio")]
        //public decimal? Case_Fatality_Ratio { get; set; }
        //FIPS,Admin2,Province_State,Country_Region,Last_Update,Lat,Long_,Confirmed,Deaths,Recovered,Active,Combined_Key,Incident_Rate,Case_Fatality_Ratio
    }
}