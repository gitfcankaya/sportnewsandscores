using SportNewsAndScores.Core.Entities;

namespace SportNewsAndScores.Infrastructure.Data;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        // Database is created via migrations in Program.cs
        
        if (context.Sports.Any())
        {
            return; // DB has been seeded
        }

        var sports = new Sport[]
        {
            new Sport { Name = "Football", NameTr = "Futbol", Icon = "⚽" },
            new Sport { Name = "Basketball", NameTr = "Basketbol", Icon = "🏀" },
            new Sport { Name = "Tennis", NameTr = "Tenis", Icon = "🎾" },
            new Sport { Name = "Volleyball", NameTr = "Voleybol", Icon = "🏐" },
            new Sport { Name = "Formula 1", NameTr = "Formula 1", Icon = "🏎️" }
        };
        context.Sports.AddRange(sports);
        context.SaveChanges();

        var countries = new Country[]
        {
            new Country { Name = "Turkey", Code = "TR", FlagUrl = "🇹🇷" },
            new Country { Name = "England", Code = "GB", FlagUrl = "🇬🇧" },
            new Country { Name = "Spain", Code = "ES", FlagUrl = "🇪🇸" },
            new Country { Name = "Germany", Code = "DE", FlagUrl = "🇩🇪" },
            new Country { Name = "France", Code = "FR", FlagUrl = "🇫🇷" },
            new Country { Name = "Italy", Code = "IT", FlagUrl = "🇮🇹" },
            new Country { Name = "Brazil", Code = "BR", FlagUrl = "🇧🇷" },
            new Country { Name = "Argentina", Code = "AR", FlagUrl = "🇦🇷" },
            new Country { Name = "USA", Code = "US", FlagUrl = "🇺🇸" }
        };
        context.Countries.AddRange(countries);
        context.SaveChanges();

        var footballSportId = sports.First(s => s.Name == "Football").Id;
        var basketballSportId = sports.First(s => s.Name == "Basketball").Id;
        
        var turkeyId = countries.First(c => c.Code == "TR").Id;
        var englandId = countries.First(c => c.Code == "GB").Id;
        var spainId = countries.First(c => c.Code == "ES").Id;

        var teams = new Team[]
        {
            new Team { Name = "Galatasaray", LogoUrl = "", CountryId = turkeyId },
            new Team { Name = "Fenerbahçe", LogoUrl = "", CountryId = turkeyId },
            new Team { Name = "Beşiktaş", LogoUrl = "", CountryId = turkeyId },
            new Team { Name = "Manchester United", LogoUrl = "", CountryId = englandId },
            new Team { Name = "Liverpool", LogoUrl = "", CountryId = englandId },
            new Team { Name = "Real Madrid", LogoUrl = "", CountryId = spainId },
            new Team { Name = "Barcelona", LogoUrl = "", CountryId = spainId }
        };
        context.Teams.AddRange(teams);
        context.SaveChanges();

        var galatasarayId = teams.First(t => t.Name == "Galatasaray").Id;
        var fenerbahceId = teams.First(t => t.Name == "Fenerbahçe").Id;

        var players = new Player[]
        {
            new Player 
            { 
                FirstName = "Mauro", 
                LastName = "Icardi", 
                Position = "Forward", 
                DateOfBirth = new DateTime(1993, 2, 19),
                PhotoUrl = "",
                TeamId = galatasarayId,
                CountryId = countries.First(c => c.Code == "AR").Id
            },
            new Player 
            { 
                FirstName = "Dušan", 
                LastName = "Tadić", 
                Position = "Midfielder", 
                DateOfBirth = new DateTime(1988, 11, 20),
                PhotoUrl = "",
                TeamId = fenerbahceId,
                CountryId = turkeyId
            }
        };
        context.Players.AddRange(players);
        context.SaveChanges();

        var matches = new Match[]
        {
            new Match
            {
                SportId = footballSportId,
                HomeTeamId = galatasarayId,
                AwayTeamId = fenerbahceId,
                MatchDate = DateTime.UtcNow.AddDays(7),
                Status = "Scheduled",
                Venue = "Türk Telekom Stadium"
            },
            new Match
            {
                SportId = footballSportId,
                HomeTeamId = fenerbahceId,
                AwayTeamId = galatasarayId,
                MatchDate = DateTime.UtcNow.AddDays(-3),
                HomeScore = 2,
                AwayScore = 1,
                Status = "Finished",
                Venue = "Şükrü Saracoğlu Stadium"
            }
        };
        context.Matches.AddRange(matches);
        context.SaveChanges();

        var news = new News[]
        {
            new News
            {
                Title = "Galatasaray secures victory in derby match",
                Content = "Galatasaray defeated their rivals in a thrilling match that kept fans on the edge of their seats.",
                SourceUrl = "https://example.com/news1",
                ImageUrl = "",
                Language = "en",
                SportId = footballSportId,
                MatchId = matches[1].Id
            },
            new News
            {
                Title = "Icardi scores hat-trick in stunning performance",
                Content = "Mauro Icardi showcased his exceptional skills with three goals in the latest match.",
                SourceUrl = "https://example.com/news2",
                ImageUrl = "",
                Language = "en",
                SportId = footballSportId,
                PlayerId = players[0].Id
            }
        };
        context.News.AddRange(news);
        context.SaveChanges();
    }
}
