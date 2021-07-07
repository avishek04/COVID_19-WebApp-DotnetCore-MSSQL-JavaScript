using System.ComponentModel.DataAnnotations;

namespace COVID_19.Models
{
    public class SurveyResponse
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public bool A1 { get; set; }
        public bool A2 { get; set; }
        public bool A3 { get; set; }
        public bool A4 { get; set; }
        public bool A5 { get; set; }
        public bool A6 { get; set; }
        public bool A7 { get; set; }
        public bool A8 { get; set; }
        public bool A9 { get; set; }
        public string Comments { get; set; }
    }
}
