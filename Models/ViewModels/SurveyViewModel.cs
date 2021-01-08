using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COVID_19.Models.ViewModels
{
    public class SurveyViewModel
    {
        public IEnumerable<SurveyQuestions> WFHSurveyQuestions { get; set; }
        public IEnumerable<SurveyQuestions> SocialSurveyQuestions { get; set; }
        public SurveyUserData SurveyData { get; set; }
    }
}
