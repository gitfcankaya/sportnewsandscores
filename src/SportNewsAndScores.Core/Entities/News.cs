namespace SportNewsAndScores.Core.Entities;

public class News : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string SourceUrl { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public int SportId { get; set; }
    public Sport Sport { get; set; } = null!;
    public int? PlayerId { get; set; }
    public Player? Player { get; set; }
    public int? MatchId { get; set; }
    public Match? Match { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
