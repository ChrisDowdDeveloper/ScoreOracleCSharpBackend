namespace ScoreOracleCSharp.Models
{
    public class Sport
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string League { get; set; } = string.Empty;
        public string LogoURL { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;

        //Relations
        public List<Team> Teams { get; set; } = new List<Team>();
        public List<Game> Games { get; set; } = new List<Game>();
        public List<Player> PlayersInSport { get; set; } = new List<Player>();
        public List<Leaderboard> LeaderboardsBySport { get;set; } = new List<Leaderboard>();

    }
}
