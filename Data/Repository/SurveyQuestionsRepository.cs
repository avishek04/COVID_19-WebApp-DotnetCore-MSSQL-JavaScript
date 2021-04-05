using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COVID_19.Models;
using Microsoft.EntityFrameworkCore;

namespace COVID_19.Data.Repository
{
    /*public class SurveyQuestionsRepository : ISurveyQuestionsRepository
    {
        public AppDbContext _appDbContext;

        public SurveyQuestionsRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<SurveyQuestions> AllQuestions
        {
            get
            {
                return _appDbContext.SurveyQuestions;
            }
        }

        public IEnumerable<SurveyQuestions> WFHQuestions
        {
            get
            {
                return _appDbContext.SurveyQuestions.Where(s => s.SurveyQuestionType == SurveyQuestions.SurveyType.WFH);
            }
        }
        public IEnumerable<SurveyQuestions> SocialQuestions
        {
            get
            {
                return _appDbContext.SurveyQuestions.Where(s => s.SurveyQuestionType == SurveyQuestions.SurveyType.Social);
            }
        }
    }*/
}