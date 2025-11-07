namespace SportNewsAndScores.Core.Entities;

public class Comment : BaseEntity
{
    public string Content { get; set; } = string.Empty;
    public string GeneratedBy { get; set; } = string.Empty; // OpenAI or Gemini
    public int? NewsId { get; set; }
    public News? News { get; set; }
    public int? MatchId { get; set; }
    public Match? Match { get; set; }
}
