using System.ComponentModel.DataAnnotations;

namespace COVID_19.Models
{
    public class Country
    {
        [Key]
        public int id { get; set; }

        public string country_name { get; set; }

        public string continent { get; set; }

        public int population { get; set; }

        public string iso2 { get; set; }

        public string iso3 { get; set; }
    }
}
