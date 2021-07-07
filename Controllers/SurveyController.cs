using COVID_19.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using COVID_19.Models;
using System.Collections.Generic;
using COVID_19.Models.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;

namespace COVID_19.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SurveyController : ControllerBase
    {
        public readonly ISurveyRepository _surveyRepository;
        private readonly IMemoryCache _memoryCache;

        public SurveyController(ISurveyRepository surveyRepository, IMemoryCache memoryCache)
        {
            _surveyRepository = surveyRepository;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public ActionResult<List<Questions>> SurveyQuestions()
        {
            List<Questions> surveyQuestion = new List<Questions>();
            bool isExist = _memoryCache.TryGetValue("SurveyQuestions", out surveyQuestion);

            if (!isExist)
            {
                surveyQuestion = _surveyRepository.GetSurveyQuestions().ToList();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(1800));
                _memoryCache.Set("SurveyQuestions", surveyQuestion, cacheEntryOptions);
            }

            return Ok(surveyQuestion);
        }

        [HttpGet]
        public ActionResult<List<SurveyResponse>> SurveyData()
        {
            List<SurveyResponse> surveyResponse = new List<SurveyResponse>();
            bool isExist = _memoryCache.TryGetValue("SurveyData", out surveyResponse);

            if (!isExist)
            {
                surveyResponse = _surveyRepository.GetAllSurveyResponse().ToList();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(1800));
                _memoryCache.Set("SurveyData", surveyResponse, cacheEntryOptions);
            }
            return Ok(surveyResponse);
        }

        [HttpGet("{quesNum}")]
        public ActionResult<SurveyResViewModel> SurveyQuesData(string quesNum)
        {
            return Ok(_surveyRepository.GetSurveyQuesResponse(quesNum));
        }

        [HttpPost]
        public ActionResult SurveyResponse(SurveyResponse surveyResponse)
        {
            if (_surveyRepository.SendSurveyResponse(surveyResponse))
            {
                return Ok("Success");
            }
            return Ok("Fail");
        }

        [HttpGet("{pageName}")]
        public ActionResult<SurveyResViewModel> VistCount(string pageName)
        {
            return Ok(_surveyRepository.SetVisitCount(pageName));
        }
    }
}
