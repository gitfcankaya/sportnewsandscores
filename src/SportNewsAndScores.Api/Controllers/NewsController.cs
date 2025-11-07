using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportNewsAndScores.Core.Entities;
using SportNewsAndScores.Infrastructure.Data;

namespace SportNewsAndScores.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public NewsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<News>>> GetNews(
        [FromQuery] int? sportId = null,
        [FromQuery] string? language = null)
    {
        var query = _context.News
            .Include(n => n.Sport)
            .Include(n => n.Player)
            .Include(n => n.Match)
            .AsQueryable();

        if (sportId.HasValue)
        {
            query = query.Where(n => n.SportId == sportId.Value);
        }

        if (!string.IsNullOrEmpty(language))
        {
            query = query.Where(n => n.Language == language);
        }

        var news = await query.OrderByDescending(n => n.CreatedAt).Take(50).ToListAsync();
        return Ok(news);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<News>> GetNews(int id)
    {
        var news = await _context.News
            .Include(n => n.Sport)
            .Include(n => n.Player)
            .Include(n => n.Match)
            .Include(n => n.Comments)
            .FirstOrDefaultAsync(n => n.Id == id);

        if (news == null)
        {
            return NotFound();
        }

        return Ok(news);
    }

    [HttpPost]
    public async Task<ActionResult<News>> CreateNews(News news)
    {
        _context.News.Add(news);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetNews), new { id = news.Id }, news);
    }
}
