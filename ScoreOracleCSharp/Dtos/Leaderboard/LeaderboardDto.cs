using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Dtos.Leaderboard
{
    public class LeaderboardDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int SportId { get; set; }
        public string SportName { get; set; } = string.Empty;
        public List<SimpleUserScore> UserScores { get; set; } = new List<SimpleUserScore>();
    }
    public class SimpleUserScore
    {
        public string? UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public int Score { get; set; }
    }
}