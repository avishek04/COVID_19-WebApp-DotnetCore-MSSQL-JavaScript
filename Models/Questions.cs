using System.ComponentModel.DataAnnotations;

namespace COVID_19.Models
{
    public class Questions
    {
        [Key]
        public int Id { get; set; }

        public string Question { get; set; }

        public string QuestionType { get; set; }

        public string QuestionCategory { get; set; }

        public string Option1 { get; set; }

        public string Option2 { get; set; }

        public string Option3 { get; set; }
    }
}
