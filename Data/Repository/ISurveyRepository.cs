using System.Collections.Generic;
using COVID_19.Models;
using COVID_19.Models.ViewModels;

namespace COVID_19.Data.Repository
{
    public interface ISurveyRepository
    {
        List<Questions> GetSurveyQuestions();
        List<SurveyResponse> GetAllSurveyResponse();
        SurveyResViewModel GetSurveyQuesResponse(string quesNum);
        bool SendSurveyResponse(SurveyResponse surveyResponse);
        bool SetVisitCount(string pageName);
    }
}