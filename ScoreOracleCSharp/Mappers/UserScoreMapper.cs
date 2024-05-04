using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.UserScore;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Mappers
{
    public static class UserScoreMapper
    {
        public static UserScoreDto ToUserScoreDto(UserScore userScoreModel)
        {
            return new UserScoreDto
            {
                Id = userScoreModel.Id,
                UserId = userScoreModel.UserId ?? 0,
                Username = userScoreModel.User?.Username ?? "Unknown",
                LeaderboardId = userScoreModel.LeaderboardId ?? 0,
                LeaderboardName = userScoreModel.Leaderboard?.Name ?? "Unknown",
                Score = userScoreModel.Score,
                UpdatedLast = userScoreModel.UpdatedLast
            };
        }
    }
}