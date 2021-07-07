using System;
using System.ComponentModel.DataAnnotations;

namespace COVID_19.Models.ViewModels
{
    public class CovidGraphDataViewModel
    {
        public string countryName { get; set; }
        [DataType(DataType.Date)]
        public DateTime reportDate { get; set; }
        public long? dailyCases { get; set; }
        public long? totalCases { get; set; }
        public long? dailyDeaths { get; set; }
        public long? totalDeaths { get; set; }
    }
}