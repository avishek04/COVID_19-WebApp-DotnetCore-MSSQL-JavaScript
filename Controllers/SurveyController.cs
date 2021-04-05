using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COVID_19.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using COVID_19.Models.ViewModels;
using COVID_19.Models;
using Microsoft.AspNetCore.Http;

namespace COVID_19.Controllers
{
	/*[ApiController]
	[Route("api/[controller]")]
	public class SurveyController : Controller
	{
		public readonly ISurveyQuestionsRepository _surveyQuestionsRepository;
		public readonly ISurveyUserDataRepository _surveyUserDataRepository;
		public SurveyController(ISurveyQuestionsRepository surveyQuestionsRepository, ISurveyUserDataRepository surveyUserDataRepository)
		{
			_surveyQuestionsRepository = surveyQuestionsRepository;
			_surveyUserDataRepository = surveyUserDataRepository;
		}
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IEnumerable<SurveyQuestions>> Get()
		{
			//var surveyVM = new SurveyViewModel
			//{
			return Ok(_surveyQuestionsRepository.WFHQuestions);
			//SocialSurveyQuestions = _surveyQuestionsRepository.SocialQuestions
			//};
		}

		public IActionResult SocialSurvey()
		{
			var surveyVM = new SurveyViewModel
			{
				//WFHSurveyQuestions = _surveyQuestionsRepository.WFHQuestions,
				SocialSurveyQuestions = _surveyQuestionsRepository.SocialQuestions
			};
			return View(surveyVM);
		}

		[HttpPost]
		public IActionResult WFHSurvey(SurveyViewModel userData)
		{
			var wfhUserData = userData.SurveyData;
			wfhUserData.UserId = 1;
			_surveyUserDataRepository.AddSurveyData(wfhUserData);
			return RedirectToAction(nameof(Index));
		}
	}*/
}
