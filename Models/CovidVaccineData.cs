using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COVID_19.Models
{
    public class CovidVaccineData
    {
        [Key]
        public int id { get; set; }

        public int country_id { get; set; }

        [DataType(DataType.Date)]
        public DateTime vaccine_report_date { get; set; }

        [DataType(DataType.Date)]
        public DateTime db_update_date_vaccine { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal? daily_vaccinations { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? daily_vaccinations_per_million { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? people_vaccinated { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? people_vaccinated_per_hundred { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? people_fully_vaccinated { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? people_fully_vaccinated_per_hundred { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? total_vaccinations { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? total_vaccinations_per_hundred { get; set; }
    }
}
