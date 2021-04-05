using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COVID_19.Models
{
    public class CovidData
    {
        [Key]
        public int id { get; set; }

        public int country_id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime report_date { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime db_update_date { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal? confirmed_cases { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? death_cases { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? recovered { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? active_cases { get; set; }

        //[DataType(DataType.DateTime)]
        //public DateTime last_update { get; set; }

        //[Name("province_state")]
        //public string province_state { get; set; }

        //[Name("Country_Region")]
        //public string Country_Region { get; set; }

        //[Column(TypeName = "decimal(20, 10)")]
        //[Name("Lat")]
        //public decimal? Latitude { get; set; }

        //[Column(TypeName = "decimal(20,10)")]
        //[Name("Long_")]
        //public decimal? Longitude { get; set; }

        //[Name("Combined_Key")]
        //public string Combined_Key { get; set; }

        //[Column(TypeName = "decimal(20,10)")]
        //[Name("Incident_Rate")]
        //public decimal? Incident_Rate { get; set; }

        //[Column(TypeName = "decimal(6,3)")]
        //[Name("Case_Fatality_Ratio")]
        //public decimal? Case_Fatality_Ratio { get; set; }
        //FIPS,Admin2,Province_State,Country_Region,Last_Update,Lat,Long_,Confirmed,Deaths,Recovered,Active,Combined_Key,Incident_Rate,Case_Fatality_Ratio
    }
}