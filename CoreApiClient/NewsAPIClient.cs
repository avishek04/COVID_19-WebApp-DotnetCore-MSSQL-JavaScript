using System;
using NewsAPI;
using NewsAPI.Models;
using NewsAPI.Constants;
using System.Collections.Generic;
using COVID_19.Models;

namespace COVID_19.CoreApiClient
{
    public class NewsAPIClient : INewsAPIClient
    {
        public List<NewsArticle> FetchCovidNews(string countryCode)
        {
            var newsApiClient = new NewsApiClient("a3916796d2c5440987fd9c732915ee1b");
            var result = new List<NewsArticle>();
            var topHeadlinesReq = new TopHeadlinesRequest();
            if (countryCode != "nil")
            {
                topHeadlinesReq.Q = "covid";
                topHeadlinesReq.Country = (Countries?)(int)Enum.Parse(typeof(Countries), countryCode);
                topHeadlinesReq.Language = Languages.EN;
            }
            else
            {
                topHeadlinesReq.Q = "covid";
                topHeadlinesReq.Language = Languages.EN;
            }
            
            var articlesResponse = newsApiClient.GetTopHeadlines(topHeadlinesReq);
            if (articlesResponse.Status == Statuses.Ok)
            {
                foreach (var article in articlesResponse.Articles)
                {
                    result.Add(new NewsArticle
                    {
                        source = article.Source.Name,
                        title = article.Title,
                        description = article.Description,
                        url = article.Url,
                        urlToImage = article.UrlToImage,
                        publishedAt = article.PublishedAt
                    });
                }
            }
            return result;
        }
    }
}
