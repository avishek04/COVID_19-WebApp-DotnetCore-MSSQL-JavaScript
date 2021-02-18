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
        //public enum DevelopmentLevel
        //{
        //    Developed,
        //    Developing,
        //    Under_developed
        //}
        [Key]
        public int Id { get; set; }

        [StringLength(2)]
        public string Iso2 { get; set; }

        [StringLength(80)]
        public string Name { get; set; }

        [StringLength(80)]
        public string Nicename { get; set; }

        [StringLength(3)]
        public string Iso3 { get; set; }

        public int Numcode { get; set; }

        public int Phonecode { get; set; }
        //[Column(TypeName = "decimal(6,2)")]
        //public decimal Density { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime DateUpdated { get; set; }

        //public DevelopmentLevel Level { get; set; }
    }
}
