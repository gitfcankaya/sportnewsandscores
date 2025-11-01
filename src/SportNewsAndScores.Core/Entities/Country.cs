namespace SportNewsAndScores.Core.Entities;

public class Country : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string FlagUrl { get; set; } = string.Empty;
    public ICollection<Team> Teams { get; set; } = new List<Team>();
    public ICollection<Player> Players { get; set; } = new List<Player>();
}
