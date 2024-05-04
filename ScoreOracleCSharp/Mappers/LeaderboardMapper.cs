using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Leaderboard;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Mappers
{
    public static class LeaderboardMapper
    {
        public static LeaderboardDto ToLeaderboardDto(Leaderboard leaderboardModel)
        {
            return new LeaderboardDto
            {
                Id = leaderboardModel.Id,
                Name = leaderboardModel.Name,
                Type = leaderboardModel.Type.ToString(),
                SportId = leaderboardModel.SportId ?? 0,
                SportName = leaderboardModel.Sport?.Name ?? "Unknown",

                UserScores = leaderboardModel.ScoreByUser.Select(us => new SimpleUserScore
                {
                    UserId = us.UserId ?? 0,
                    Username = us.User?.Username ?? "Unknown",
                    Score = us.Score
                }).ToList()
            };
        }
    }
}