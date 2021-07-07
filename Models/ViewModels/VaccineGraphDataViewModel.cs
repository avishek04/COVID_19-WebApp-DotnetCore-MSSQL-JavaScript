using System;
namespace COVID_19.Models.ViewModels
{
    public class VaccineGraphDataViewModel
    {
        public string countryName { get; set; }
        public DateTime reportDate { get; set; }
        public long? dailyVaccinations { get; set; }
        //public long? dailyVaccinationsPerMillion { get; set; }
        public long? peopleVaccinated { get; set; }
        //public long? peopleVaccinatedPerHun { get; set; }
        public long? peopleFullyVaccinated { get; set; }
        //public long? peopleFullyVaccinatedPerHun { get; set; }
        public long? totalVaccinations { get; set; }
        //public long? totalVaccinationPerHun { get; set; }
    }
}
