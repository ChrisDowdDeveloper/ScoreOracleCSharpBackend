namespace ScoreOracleCSharp.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string City { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public int? SportId { get; set; }
        public Sport? Sport { get; set; }

        public string LogoURL { get; set; } = string.Empty;

        //Relations
        public List<Game> HomeGames { get; set; } = new List<Game>();
        public List<Game> AwayGames { get; set; } = new List<Game>();
        public List<Injury> InjuriesOnTeam { get; set; } = new List<Injury>();
        public List<Player> PlayersOnTeam { get; set; } = new List<Player>();
        public List<Prediction> TeamPredicted { get; set; } = new List<Prediction>();
    }
}
