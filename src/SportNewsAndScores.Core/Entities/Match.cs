namespace SportNewsAndScores.Core.Entities;

public class Match : BaseEntity
{
    public int SportId { get; set; }
    public Sport Sport { get; set; } = null!;
    public int HomeTeamId { get; set; }
    public Team HomeTeam { get; set; } = null!;
    public int AwayTeamId { get; set; }
    public Team AwayTeam { get; set; } = null!;
    public DateTime MatchDate { get; set; }
    public int? HomeScore { get; set; }
    public int? AwayScore { get; set; }
    public string Status { get; set; } = string.Empty; // Scheduled, Live, Finished
    public string Venue { get; set; } = string.Empty;
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<News> News { get; set; } = new List<News>();
}
