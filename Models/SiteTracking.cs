using System;
using System.ComponentModel.DataAnnotations;

namespace COVID_19.Models
{
    public class SiteTracking
    {
        [Key]
        public int Id { get; set; }

        public string PageName { get; set; }

        public int VisitCount { get; set; }

        [DataType(DataType.Date)]
        public DateTime? UpdateDate { get; set; }
    }
}
