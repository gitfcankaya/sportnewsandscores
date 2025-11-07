using System.ComponentModel.DataAnnotations;

namespace SportNewsAndScores.Core.Entities;

public class News : BaseEntity
{
    [Required]
    [MaxLength(500)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Content { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(1000)]
    public string SourceUrl { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(1000)]
    public string ImageUrl { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(10)]
    public string Language { get; set; } = string.Empty;
    
    public int SportId { get; set; }
    public Sport Sport { get; set; } = null!;
    
    public int? PlayerId { get; set; }
    public Player? Player { get; set; }
    
    public int? MatchId { get; set; }
    public Match? Match { get; set; }
    
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
