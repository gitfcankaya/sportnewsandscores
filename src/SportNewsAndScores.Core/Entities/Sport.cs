namespace SportNewsAndScores.Core.Entities;

public class Sport : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string NameTr { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public ICollection<News> News { get; set; } = new List<News>();
    public ICollection<Match> Matches { get; set; } = new List<Match>();
}
