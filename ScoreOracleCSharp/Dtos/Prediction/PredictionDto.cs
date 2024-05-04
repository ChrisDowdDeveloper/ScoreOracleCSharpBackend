using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Dtos.Prediction
{
    public class PredictionDto
    {
        public int Id { get; set; }
        public DateOnly PredictionDate { get; set; }
        public int GameId { get; set; }
        public DateOnly GameDate { get; set; }
        public int PredictedTeamId { get; set; }
        public string PredictedTeamName { get; set; } = string.Empty;
        public int PredictedHomeTeamScore { get; set; }
        public int PredictedAwayTeamScore { get; set; }
    }
}