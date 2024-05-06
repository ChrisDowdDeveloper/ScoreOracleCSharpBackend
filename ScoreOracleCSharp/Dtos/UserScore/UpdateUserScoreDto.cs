using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Dtos.UserScore
{
    public class UpdateUserScoreDto
    {
        public int? UserId { get; set; }
        public int? LeaderboardId { get; set; }
        public int Score { get; set; }
        public DateTime UpdatedLast { get; set; }
    }
}