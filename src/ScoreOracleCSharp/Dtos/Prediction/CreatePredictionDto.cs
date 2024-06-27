using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Dtos.Prediction
{
    public class CreatePredictionDto
    {
        public string UserId { get; set; } = string.Empty;
        public int GameId { get; set; }
        public int PredictedTeamId { get; set; }
        public int PredictedHomeTeamScore { get; set; }
        public int PredictedAwayTeamScore { get; set; }
    }
}