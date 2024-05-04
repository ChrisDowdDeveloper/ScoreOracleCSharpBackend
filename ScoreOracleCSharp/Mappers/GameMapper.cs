using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Game;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Mappers
{
    public static class GameMapper
    {
        public static GameDto ToGameDto(this Game gameModel)
        {
            return new GameDto
            {
                Id = gameModel.Id,
                GameDate = gameModel.GameDate,
                GameStatus = gameModel.GameStatus.ToString(),

                HomeTeamId = gameModel.HomeTeamId ?? 0,
                HomeTeamName = gameModel.HomeTeam?.Name ?? "Unknown",
                HomeTeamScore = gameModel.HomeTeamScore,

                AwayTeamId = gameModel.AwayTeamId ?? 0,
                AwayTeamName = gameModel.AwayTeam?.Name ?? "Unknown",
                AwayTeamScore = gameModel.AwayTeamScore,

                SportId = gameModel.SportId ?? 0,
                SportName = gameModel.Sport?.Name ?? "Unknown",

                PredictionCount = gameModel.GamePrediction.Count
            };
        }
    }
}