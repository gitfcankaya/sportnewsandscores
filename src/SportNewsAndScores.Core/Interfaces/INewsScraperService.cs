using SportNewsAndScores.Core.Entities;

namespace SportNewsAndScores.Core.Interfaces;

public interface INewsScraperService
{
    Task<List<News>> ScrapeNewsAsync(string sport, string language);
}
