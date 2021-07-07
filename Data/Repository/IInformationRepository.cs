using COVID_19.Models;
using System.Collections.Generic;

namespace COVID_19.Data.Repository
{
    public interface IInformationRepository
    {
        List<NewsArticle> GetTopCovidNewsCountry();
        List<NewsArticle> GetTopCovidNewsNYT();
    }
}
