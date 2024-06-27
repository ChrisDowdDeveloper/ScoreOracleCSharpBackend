using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Game;
using ScoreOracleCSharp.Dtos.Injury;
using ScoreOracleCSharp.Dtos.Player;
using ScoreOracleCSharp.Dtos.Prediction;

namespace ScoreOracleCSharp.Dtos.Team
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string City { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int? SportId { get; set; }
        public string SportName { get; set; } = string.Empty;
        public string LogoURL { get; set; } = string.Empty;
        public List<GameDto> HomeGames { get; set; }
        public List<GameDto> AwayGames { get; set; }
        public List<InjuryDto> InjuriesOnTeam { get; set; }
        public List<PlayerDto> PlayersOnTeam { get; set; }
        public List<PredictionDto> TeamPredicted { get; set; }
    }
}