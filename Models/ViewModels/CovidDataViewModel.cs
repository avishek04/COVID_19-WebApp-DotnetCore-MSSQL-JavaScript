using System;
namespace COVID_19.Models.ViewModels
{
    public class CovidDataViewModel
    {
        public string country_name { get; set; }

        public string iso2 { get; set; }

        public string iso3 { get; set; }

        public DateTime report_date { get; set; }

        public DateTime db_update_date { get; set; }

        public decimal? confirmed_cases { get; set; }

        public decimal? death_cases { get; set; }

        public decimal? recovered { get; set; }

        public decimal? active_cases { get; set; }
    }
}
