using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Dtos.UserScore
{
    public class UserScoreDto
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public int LeaderboardId { get; set; }
        public string LeaderboardName { get; set; } = string.Empty;
        public int Score { get; set; }
        public DateTime UpdatedLast { get; set; }
    }
}