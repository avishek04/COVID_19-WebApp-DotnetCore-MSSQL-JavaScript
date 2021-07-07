using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COVID_19.Models.ViewModels
{
    public class CovidGridDataViewModel
    {
        public string countryName { get; set; }
        public long? totalCases { get; set; }
        public long? totalDeaths { get; set; }
        public long? dailyCases { get; set; }
        public long? dailyDeaths { get; set; }
        public long? weeklyCases { get; set; }
        public long? weeklyDeaths { get; set; }
        public long? biweeklyCases { get; set; }
        public long? biweeklyDeaths { get; set; }
    }
}
