using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Dtos.Prediction
{
    public class UpdatePredictionDto
    {
        public string? UserId { get; set; }
        public DateOnly PredictionDate { get; set; }
        public int? GameId { get; set; }
        public DateOnly GameDate { get; set; }
        public int? PredictedTeamId { get; set; }
        public int? PredictedHomeTeamScore { get; set; }
        public int? PredictedAwayTeamScore { get; set; }
    }
}