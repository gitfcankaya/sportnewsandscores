using HtmlAgilityPack;
using SportNewsAndScores.Core.Entities;
using SportNewsAndScores.Core.Interfaces;

namespace SportNewsAndScores.Infrastructure.Services;

public class NewsScraperService : INewsScraperService
{
    private readonly HttpClient _httpClient;

    public NewsScraperService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<News>> ScrapeNewsAsync(string sport, string language)
    {
        var newsList = new List<News>();
        
        try
        {
            var url = $"https://www.bbc.com/sport/{sport.ToLower()}";
            var html = await _httpClient.GetStringAsync(url);
            
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            
            var newsNodes = htmlDoc.DocumentNode.SelectNodes("//article");
            
            if (newsNodes == null)
            {
                return newsList;
            }
            
            foreach (var node in newsNodes.Take(10))
            {
                var titleNode = node.SelectSingleNode(".//h2|.//h3");
                var linkNode = node.SelectSingleNode(".//a");
                var imgNode = node.SelectSingleNode(".//img");
                
                if (titleNode != null && linkNode != null)
                {
                    var news = new News
                    {
                        Title = titleNode.InnerText.Trim(),
                        Content = titleNode.InnerText.Trim(),
                        SourceUrl = linkNode.GetAttributeValue("href", ""),
                        ImageUrl = imgNode?.GetAttributeValue("src", "") ?? "",
                        Language = language
                    };
                    newsList.Add(news);
                }
            }
        }
        catch (Exception)
        {
            // Log error - for now, return empty list
        }
        
        return newsList;
    }
}
