using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace COVID_19.Models
{
    public class Country
    {
        public enum DevelopmentLevel
        {
            Developed,
            Developing,
            Under_developed
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string CountryName { get; set; }

        [StringLength(3)]
        public string CountryCode { get; set; }

        [StringLength(50)]
        public string Capital { get; set; }

        public int Population { get; set; }

        public int HumanDevelopmentIndex_HDI { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal Density { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateUpdated { get; set; }

        public DevelopmentLevel Level { get; set; }
    }
}
