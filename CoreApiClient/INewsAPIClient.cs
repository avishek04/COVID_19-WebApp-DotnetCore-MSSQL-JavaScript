using COVID_19.Models;
using System.Collections.Generic;

namespace COVID_19.CoreApiClient
{
    public interface INewsAPIClient
    {
        List<NewsArticle> FetchCovidNews();
        List<NewsArticle> FetchNYTHealthNews();
    }
}
