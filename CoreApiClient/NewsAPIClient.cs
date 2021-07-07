using System;
using System.Collections.Generic;
using COVID_19.Models;
using System.Xml;

namespace COVID_19.CoreApiClient
{
    public class NewsAPIClient : INewsAPIClient
    {
        public List<NewsArticle> FetchCovidNews()
        {
            List<NewsArticle> newsList = new List<NewsArticle>();
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load("https://news.google.com/rss/search?q=covid&hl=en-IN&gl=IN&ceid=IN:en");
                XmlNodeList xmlNodes = xmlDoc.SelectNodes("rss/channel/item");

                foreach (XmlNode xmlNode in xmlNodes)
                {
                    NewsArticle news = new NewsArticle();

                    XmlNode xmlSubNode = xmlNode.SelectSingleNode("title");
                    string title = xmlSubNode != null ? xmlSubNode.InnerText : "";
                    news.title = title;

                    xmlSubNode = xmlNode.SelectSingleNode("link");
                    string link = xmlSubNode != null ? xmlSubNode.InnerText : "";
                    news.Url = link;

                    xmlSubNode = xmlNode.SelectSingleNode("pubDate");
                    string pubDate = xmlSubNode != null ? xmlSubNode.InnerText : "";
                    news.publishedAt = pubDate;

                    //xmlSubNode = xmlNode.SelectSingleNode("description");
                    //string description = xmlSubNode != null ? xmlSubNode.InnerText : "";
                    //news.description = description;

                    xmlSubNode = xmlNode.SelectSingleNode("source");
                    string source = xmlSubNode != null ? xmlSubNode.InnerText : "";
                    news.source = source;

                    newsList.Add(news);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return newsList;
        }

        public List<NewsArticle> FetchNYTHealthNews()
        {
            List<NewsArticle> newsList = new List<NewsArticle>();
            XmlDocument nytXml = new XmlDocument();

            try
            {
                nytXml.Load("https://rss.nytimes.com/services/xml/rss/nyt/Health.xml");
                XmlNodeList nytNodes = nytXml.SelectNodes("rss/channel/item");
                var nodes = nytXml.GetElementsByTagName("media:content");
                int i = 0;
                foreach (XmlNode xmlNode in nytNodes)
                {
                    NewsArticle news = new NewsArticle();

                    XmlNode xmlSubNode = xmlNode.SelectSingleNode("title");
                    string title = xmlSubNode != null ? xmlSubNode.InnerText : "";
                    news.title = title;

                    xmlSubNode = xmlNode.SelectSingleNode("link");
                    string link = xmlSubNode != null ? xmlSubNode.InnerText : "";
                    news.Url = link;

                    xmlSubNode = xmlNode.SelectSingleNode("description");
                    string description = xmlSubNode != null ? xmlSubNode.InnerText : "";
                    news.description = description;

                    xmlSubNode = xmlNode.SelectSingleNode("pubDate");
                    string pubDate = xmlSubNode != null ? xmlSubNode.InnerText : "";
                    news.publishedAt = pubDate;

                    //xmlSubNode = xmlNode.SelectSingleNode("media");
                    if (xmlNode["media:content"] != null)
                    {
                        var urlToImage = nodes[i].Attributes["url"].InnerText;
                        news.urlToImage = urlToImage;
                        i++;
                    }
                    //xmlSubNode = xmlNode.SelectSingleNode("source");
                    //string source = xmlSubNode != null ? xmlSubNode.InnerText : "";
                    //news.source = source;
                    newsList.Add(news);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return newsList;
        }

        //NewsAPI
        //public List<NewsArticle> FetchCovidNews(string countryCode)
        //{
        //    List<NewsArticle> result = new List<NewsArticle>();
        //    List<NewsArticle> secndResult = new List<NewsArticle>();
        //    //List<NewsArticle> thrdResult = new List<NewsArticle>();
        //    List<NewsArticle> fourthResult = new List<NewsArticle>();

        //    try
        //    {
        //        var topic = "covid";
        //        result.AddRange(GetNewsApiResponse(countryCode, topic));


        //        topic = "corona";
        //        var coronaRes = GetNewsApiResponse(countryCode, topic);
        //        foreach (var news in coronaRes)
        //        {
        //            var isPresent = false;
        //            foreach (var covidNews in result)
        //            {
        //                if (news.title == covidNews.title)
        //                {
        //                    isPresent = true;
        //                    break;
        //                }
        //            }
        //            if (!isPresent)
        //            {
        //                secndResult.Add(news);
        //            }
        //        }
        //        result.AddRange(secndResult);

        //        //topic = "virus";
        //        //var virusRes = GetNewsApiResponse(countryCode, topic);
        //        //foreach (var news in virusRes)
        //        //{
        //        //    var isPresent = false;
        //        //    foreach (var covidNews in result)
        //        //    {
        //        //        if (news.title == covidNews.title)
        //        //        {
        //        //            isPresent = true;
        //        //            break;
        //        //        }
        //        //    }
        //        //    if (!isPresent)
        //        //    {
        //        //        thrdResult.Add(news);
        //        //    }
        //        //}
        //        //result.AddRange(thrdResult);

        //        topic = "vaccine";
        //        var vaccineRes = GetNewsApiResponse(countryCode, topic);
        //        foreach (var news in vaccineRes)
        //        {
        //            var isPresent = false;
        //            foreach (var covidNews in result)
        //            {
        //                if (news.title == covidNews.title)
        //                {
        //                    isPresent = true;
        //                    break;
        //                }
        //            }
        //            if (!isPresent)
        //            {
        //                fourthResult.Add(news);
        //            }
        //        }
        //        result.AddRange(fourthResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result;
        //}

        //public List<NewsArticle> GetNewsApiResponse(string countryCode, string topic)
        //{
        //    NewsApiClient newsApiClient = new NewsApiClient("a3916796d2c5440987fd9c732915ee1b");
        //    TopHeadlinesRequest topHeadlinesReq = new TopHeadlinesRequest();
        //    ArticlesResult articlesResponse = new ArticlesResult();
        //    List<NewsArticle> result = new List<NewsArticle>();

        //    try
        //    {
        //        topHeadlinesReq.Language = Languages.EN;
        //        topHeadlinesReq.Q = topic;
        //        if (countryCode != "nil")
        //        {
        //            topHeadlinesReq.Country = (Countries?)(int)Enum.Parse(typeof(Countries), countryCode);
        //        }

        //        articlesResponse = newsApiClient.GetTopHeadlines(topHeadlinesReq);
        //        if (articlesResponse.Status == Statuses.Ok)
        //        {
        //            foreach (var article in articlesResponse.Articles)
        //            {
        //                result.Add(new NewsArticle
        //                {
        //                    source = article.Source.Name,
        //                    title = article.Title,
        //                    description = article.Description,
        //                    Url = article.Url,
        //                    urlToImage = article.UrlToImage,
        //                    publishedAt = article.PublishedAt
        //                });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result;
        //}
        //NewsAPI
    }
}
