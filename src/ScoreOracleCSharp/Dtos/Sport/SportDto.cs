using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Game;
using ScoreOracleCSharp.Dtos.Leaderboard;
using ScoreOracleCSharp.Dtos.Player;
using ScoreOracleCSharp.Dtos.Team;

namespace ScoreOracleCSharp.Dtos.Sport
{
    public class SportDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string League { get; set; } = string.Empty;
        public string LogoURL { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
        public List<TeamDto> Teams { get; set; }
        public List<GameDto> Games { get; set; }
        public List<PlayerDto> PlayersInSport { get; set; }
        public List<LeaderboardDto> LeaderboardsBySport { get; set; }
    }
}