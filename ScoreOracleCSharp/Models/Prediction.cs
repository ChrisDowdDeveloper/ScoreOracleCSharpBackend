﻿namespace ScoreOracleCSharp.Models
{
    public class Prediction
    {
        public int Id { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }

        public int? GameId { get; set; }
        public Game? Game { get; set; }

        public int? PredictedTeamId { get; set; }
        public Team? Team { get; set; }

        public int PredictedHomeTeamScore { get; set; }
        public int PredictedAwayTeamScore { get; set; }
        public DateOnly PredictionDate { get; set; } = new DateOnly();
    }
}
