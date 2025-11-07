using System.ComponentModel.DataAnnotations;

namespace SportNewsAndScores.Core.Entities;

public class Sport : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string NameTr { get; set; } = string.Empty;
    
    [MaxLength(10)]
    public string Icon { get; set; } = string.Empty;
    
    public ICollection<News> News { get; set; } = new List<News>();
    public ICollection<Match> Matches { get; set; } = new List<Match>();
}
