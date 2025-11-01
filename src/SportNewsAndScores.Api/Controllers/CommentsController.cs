using Microsoft.AspNetCore.Mvc;
using SportNewsAndScores.Core.Entities;
using SportNewsAndScores.Core.Interfaces;
using SportNewsAndScores.Infrastructure.Data;

namespace SportNewsAndScores.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IAICommentService _aiCommentService;

    public CommentsController(ApplicationDbContext context, IAICommentService aiCommentService)
    {
        _context = context;
        _aiCommentService = aiCommentService;
    }

    [HttpPost("generate")]
    public async Task<ActionResult<Comment>> GenerateComment([FromBody] CommentRequest request)
    {
        var commentText = await _aiCommentService.GenerateCommentAsync(request.Content, request.Provider);
        
        var comment = new Comment
        {
            Content = commentText,
            GeneratedBy = request.Provider,
            NewsId = request.NewsId,
            MatchId = request.MatchId
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return Ok(comment);
    }
}

public record CommentRequest(string Content, string Provider, int? NewsId, int? MatchId);
