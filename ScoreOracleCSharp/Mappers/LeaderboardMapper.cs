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
                    UserId = us.UserId,
                    Username = us.User?.UserName ?? "Unknown",
                    Score = us.Score
                }).ToList()
            };
        }

        public static Leaderboard ToLeaderboardFromCreateDTO(CreateLeaderboardDto leaderboardDto)
        {
            return new Leaderboard
            {
                Type = leaderboardDto.Type,
                Name = leaderboardDto.Name,
                SportId = leaderboardDto.SportId
            };
        }
    }
}