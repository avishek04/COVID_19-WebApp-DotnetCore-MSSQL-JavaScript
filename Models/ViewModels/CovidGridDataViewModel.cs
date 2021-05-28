using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COVID_19.Models.ViewModels
{
    public class CovidGridDataViewModel
    {
        public string country_name { get; set; }
        public long? totalCaseCount { get; set; }
        public long? activeCaseCount { get; set; }
        public long? recoveredCaseCount { get; set; }
        public long? lastDayCaseCount { get; set; }
        public long? lastTwoWeekCaseCount { get; set; }
        public long? casesPerMillion { get; set; }
        public long? totalDeathCaseCount { get; set; }
    }
}
