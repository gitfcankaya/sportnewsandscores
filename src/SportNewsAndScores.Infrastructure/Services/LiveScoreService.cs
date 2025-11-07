using Microsoft.EntityFrameworkCore;
using SportNewsAndScores.Core.Entities;
using SportNewsAndScores.Core.Interfaces;
using SportNewsAndScores.Infrastructure.Data;

namespace SportNewsAndScores.Infrastructure.Services;

public class LiveScoreService : ILiveScoreService
{
    private readonly ApplicationDbContext _context;

    public LiveScoreService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Match>> GetLiveMatchesAsync()
    {
        return await _context.Matches
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Sport)
            .Where(m => m.Status == "Live")
            .ToListAsync();
    }

    public async Task UpdateMatchScoreAsync(int matchId, int homeScore, int awayScore)
    {
        var match = await _context.Matches.FindAsync(matchId);
        if (match != null)
        {
            match.HomeScore = homeScore;
            match.AwayScore = awayScore;
            match.Status = "Live";
            await _context.SaveChangesAsync();
        }
    }
}
