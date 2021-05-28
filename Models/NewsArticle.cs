using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COVID_19.Models
{
    public class NewsArticle
    {
        public string? source { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public string? url { get; set; }
        public string? urlToImage { get; set; }
        public DateTime? publishedAt { get; set; }
    }
}
