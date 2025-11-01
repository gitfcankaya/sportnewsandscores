using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportNewsAndScores.Core.Entities;
using SportNewsAndScores.Infrastructure.Data;

namespace SportNewsAndScores.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SportsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SportsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Sport>>> GetSports()
    {
        var sports = await _context.Sports.ToListAsync();
        return Ok(sports);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Sport>> GetSport(int id)
    {
        var sport = await _context.Sports.FindAsync(id);

        if (sport == null)
        {
            return NotFound();
        }

        return Ok(sport);
    }
}
