using COVID_19.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COVID_19.Data;

namespace COVID_19.Data.Repository
{
    public class SurveyUserDataRepository : ISurveyUserDataRepository
    {
        public AppDbContext _appDbContext;

        public SurveyUserDataRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<SurveyUserData> AllSurveyData
        {
            get
            {
                return _appDbContext.SurveyUserData;
            }
        }

        public SurveyUserData GetSurveyDataByUserId(int userId)
        {
            return _appDbContext.SurveyUserData.FirstOrDefault(s => s.UserId == userId);
        }

        public IEnumerable<SurveyUserData> GetSurveyUserDataByType(string surveyType)
        {
            IEnumerable<SurveyUserData> surveyUserDataList = from data in _appDbContext.SurveyUserData
                                                             where data.SurveyQuestionsType == surveyType
                                                             select data;
            return surveyUserDataList;
        }

        public void AddSurveyData(SurveyUserData userData)
        {
            userData.UserId = 1;
            _appDbContext.SurveyUserData.Add(userData);
            _appDbContext.SaveChanges();
        }
    }
}
