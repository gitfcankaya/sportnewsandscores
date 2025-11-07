using SportNewsAndScores.Core.Entities;

namespace SportNewsAndScores.Core.Interfaces;

public interface ILiveScoreService
{
    Task<List<Match>> GetLiveMatchesAsync();
    Task UpdateMatchScoreAsync(int matchId, int homeScore, int awayScore);
}
