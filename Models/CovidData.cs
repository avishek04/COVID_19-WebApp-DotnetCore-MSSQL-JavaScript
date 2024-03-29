﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COVID_19.Models
{
    public class CovidData
    {
        [Key]
        public int id { get; set; }

        public int country_id { get; set; }

        [DataType(DataType.Date)]
        public DateTime report_date { get; set; }

        [DataType(DataType.Date)]
        public DateTime db_update_date { get; set; }

        [Column(TypeName = "decimal(20, 5)")]
        public decimal? new_cases { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? new_deaths { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? weekly_cases { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? weekly_deaths { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? biweekly_cases { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? biweekly_deaths { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? total_cases { get; set; }

        [Column(TypeName = "decimal(20,5)")]
        public decimal? total_deaths { get; set; }
    }
}