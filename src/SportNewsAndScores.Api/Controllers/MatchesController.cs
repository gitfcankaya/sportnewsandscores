using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportNewsAndScores.Core.Entities;
using SportNewsAndScores.Core.Interfaces;
using SportNewsAndScores.Infrastructure.Data;

namespace SportNewsAndScores.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILiveScoreService _liveScoreService;

    public MatchesController(ApplicationDbContext context, ILiveScoreService liveScoreService)
    {
        _context = context;
        _liveScoreService = liveScoreService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Match>>> GetMatches(
        [FromQuery] int? sportId = null,
        [FromQuery] string? status = null)
    {
        var query = _context.Matches
            .Include(m => m.Sport)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .AsQueryable();

        if (sportId.HasValue)
        {
            query = query.Where(m => m.SportId == sportId.Value);
        }

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(m => m.Status == status);
        }

        var matches = await query.OrderByDescending(m => m.MatchDate).Take(50).ToListAsync();
        return Ok(matches);
    }

    [HttpGet("live")]
    public async Task<ActionResult<IEnumerable<Match>>> GetLiveMatches()
    {
        var matches = await _liveScoreService.GetLiveMatchesAsync();
        return Ok(matches);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Match>> GetMatch(int id)
    {
        var match = await _context.Matches
            .Include(m => m.Sport)
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Comments)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (match == null)
        {
            return NotFound();
        }

        return Ok(match);
    }

    [HttpPut("{id}/score")]
    public async Task<IActionResult> UpdateScore(int id, [FromBody] ScoreUpdate scoreUpdate)
    {
        await _liveScoreService.UpdateMatchScoreAsync(id, scoreUpdate.HomeScore, scoreUpdate.AwayScore);
        return NoContent();
    }
}

public record ScoreUpdate(int HomeScore, int AwayScore);
