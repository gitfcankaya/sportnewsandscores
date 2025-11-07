using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportNewsAndScores.Core.Entities;
using SportNewsAndScores.Infrastructure.Data;

namespace SportNewsAndScores.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PlayersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
    {
        var players = await _context.Players
            .Include(p => p.Team)
            .Include(p => p.Country)
            .ToListAsync();
        return Ok(players);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Player>> GetPlayer(int id)
    {
        var player = await _context.Players
            .Include(p => p.Team)
            .Include(p => p.Country)
            .Include(p => p.News)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (player == null)
        {
            return NotFound();
        }

        return Ok(player);
    }
}
