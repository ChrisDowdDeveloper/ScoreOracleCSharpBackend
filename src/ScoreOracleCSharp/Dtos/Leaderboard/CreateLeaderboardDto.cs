using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Dtos.Leaderboard
{
    public class CreateLeaderboardDto
    {
        public string Name { get; set; } = string.Empty;
        public int SportId { get; set; }
        public LeaderboardType Type { get; set; } = LeaderboardType.PRIVATE;
    }
}