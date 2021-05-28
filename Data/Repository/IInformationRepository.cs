using COVID_19.Models;
using System.Collections.Generic;

namespace COVID_19.Data.Repository
{
    public interface IInformationRepository
    {
        IEnumerable<NewsArticle> GetTopCovidNewsCountry(string country);
    }
}
