using COVID_19.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using COVID_19.Data.Repository;
using System.Linq;
using System.Threading.Tasks;

namespace COVID_19.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class InformationController : ControllerBase
    {
        public IInformationRepository _informationRepository;
        public InformationController(IInformationRepository informationRepository)
        {
            _informationRepository = informationRepository;
        }

        [HttpGet("{country}")]
        public ActionResult<IEnumerable<NewsArticle>> TopCovidNewsCountry(string country)
        {
            var countryCovidNews = _informationRepository.GetTopCovidNewsCountry(country).ToList();
            return Ok(countryCovidNews);
        }

        //public List<> TopTweetsCovid()
        //{

        //}
    }
}
