using System;

namespace COVID_19.Models.ViewModels
{
    public class CovidGraphDataViewModel
    {
        public string country_name { get; set; }
        public DateTime report_date { get; set; }
        public long? new_cases { get; set; }
        public long? active_cases { get; set; }
        public long? new_deaths { get; set; }
    }
}