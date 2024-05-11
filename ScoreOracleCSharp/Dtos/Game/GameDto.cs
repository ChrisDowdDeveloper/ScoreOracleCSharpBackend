using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Prediction;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Dtos.Game
{
    public class GameDto
    {
        public int Id { get; set; }
        public DateOnly GameDate { get; set; }
        public string GameStatus { get; set; } = string.Empty;

        public int HomeTeamId { get; set; }
        public string HomeTeamName { get; set; } = string.Empty;
        public int HomeTeamScore { get; set; }

        public int AwayTeamId { get; set; }
        public string AwayTeamName { get; set; } = string.Empty;
        public int AwayTeamScore { get; set; }

        public int SportId { get; set; }
        public string SportName { get; set; } = string.Empty;

        public int PredictionCount { get; set; }
        public List<PredictionDto> Predictions { get; set; }
    }
}