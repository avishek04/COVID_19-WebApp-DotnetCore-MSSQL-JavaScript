using COVID_19.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using COVID_19.Models.ViewModels;

namespace COVID_19.Data.Repository
{
    public class SurveyRepository : ISurveyRepository
    {
        private AppDbContext _appDbContext;
        private IServiceScopeFactory _serviceScopeFactory;

        public SurveyRepository(AppDbContext appDbContext, IServiceScopeFactory serviceScopeFactory)
        {
            _appDbContext = appDbContext;
            _serviceScopeFactory = serviceScopeFactory;
        }

        private IEnumerable<Questions> AllQuestions
        {
            get
            {
                return _appDbContext.questions;
            }
        }

        private IEnumerable<SurveyResponse> AllResponse
        {
            get
            {
                return _appDbContext.surveyResponse;
            }
        }

        public List<Questions> GetSurveyQuestions()
        {
            List<Questions> allQuestions = new List<Questions>();
            try
            {
                allQuestions = AllQuestions.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return allQuestions;
        }

        public List<SurveyResponse> GetAllSurveyResponse()
        {
            List<SurveyResponse> allSurveyResponse = new List<SurveyResponse>();
            try
            {
                allSurveyResponse = AllResponse.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return allSurveyResponse;
        }

        public SurveyResViewModel GetSurveyQuesResponse(string quesNum)
        {
            List<SurveyResponse> surveyResponse = new List<SurveyResponse>();
            SurveyResViewModel quesResp = new SurveyResViewModel();

            try
            {
                surveyResponse = GetAllSurveyResponse();
                var respCount = surveyResponse.Count();
                IEnumerable<bool> ansCol;

                if (quesNum == "q1")
                {
                    ansCol = surveyResponse.Select(x => x.A1);
                }
                else if (quesNum == "q2")
                {
                    ansCol = surveyResponse.Select(x => x.A2);
                }
                else if (quesNum == "q3")
                {
                    ansCol = surveyResponse.Select(x => x.A3);
                }
                else if (quesNum == "q4")
                {
                    ansCol = surveyResponse.Select(x => x.A4);
                }
                else if (quesNum == "q5")
                {
                    ansCol = surveyResponse.Select(x => x.A5);
                }
                else if (quesNum == "q6")
                {
                    ansCol = surveyResponse.Select(x => x.A6);
                }
                else if (quesNum == "q7")
                {
                    ansCol = surveyResponse.Select(x => x.A7);
                }
                else if (quesNum == "q8")
                {
                    ansCol = surveyResponse.Select(x => x.A8);
                }
                else
                {
                    ansCol = surveyResponse.Select(x => x.A9);
                }

                var posResp = ansCol.Where(x => x == true).Count();
                var negResp = ansCol.Where(x => x == false).Count();

                quesResp.positiveAns = posResp;
                quesResp.negativeAns = negResp;
                quesResp.totalAns = respCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return quesResp;
        }

        public bool SendSurveyResponse(SurveyResponse surveyResponse)
        {
            var retVal = 0;
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var checkDuplicate = AllResponse.Where(response => response.Email == surveyResponse.Email).Count();

                    if (!(checkDuplicate > 0))
                    {
                        dbContext.surveyResponse.Add(surveyResponse);
                        retVal = dbContext.SaveChanges();
                        if (retVal == 1)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public bool SetVisitCount(string pageName)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var retVal = 0;
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var countTracking = dbContext.siteTracking.Where(x => x.PageName == pageName).FirstOrDefault();
                    countTracking.VisitCount += 1;
                    countTracking.UpdateDate = DateTime.UtcNow;
                    retVal = dbContext.SaveChanges();

                    if (retVal == 1)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }
    }
}
