namespace SportNewsAndScores.Core.Entities;

public class Player : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string PhotoUrl { get; set; } = string.Empty;
    public int? TeamId { get; set; }
    public Team? Team { get; set; }
    public int CountryId { get; set; }
    public Country Country { get; set; } = null!;
    public ICollection<News> News { get; set; } = new List<News>();
}
