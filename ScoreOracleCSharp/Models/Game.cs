namespace ScoreOracleCSharp.Models
{
    public enum GameStatus
    {
        SCHEDULED,
        PLAYING,
        COMPLETED,
        POSTPONED
    }
    public class Game
    {
        public int Id { get; set; }

        public int? HomeTeamId { get; set; }
        public Team? HomeTeam { get; set; }

        public int? AwayTeamId { get; set; }
        public Team? AwayTeam { get; set; }


        public DateOnly GameDate { get; set; }

        public int? SportId { get; set; }
        public Sport? Sport { get; set; }

        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
        public GameStatus GameStatus { get; set; } = GameStatus.SCHEDULED;
        public bool ScoresUpdated { get; set; } = false;

        //Relations
        public List<Prediction> GamePrediction { get; set; } = new List<Prediction>();

    }
}
