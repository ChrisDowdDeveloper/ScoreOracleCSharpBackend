using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Prediction;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Mappers
{
    public static class PredictionMapper
    {
        public static PredictionDto ToPredictionDto(Prediction predictionModel)
        {
            return new PredictionDto
            {
                Id = predictionModel.Id,
                PredictionDate = predictionModel.PredictionDate,
                GameId = predictionModel.GameId ?? 0,
                GameDate = predictionModel.Game?.GameDate ?? new DateOnly(),
                PredictedTeamId = predictionModel.PredictedTeamId ?? 0,
                PredictedTeamName = predictionModel.Team?.Name ?? "Unknown",
                PredictedHomeTeamScore = predictionModel.PredictedHomeTeamScore,
                PredictedAwayTeamScore = predictionModel.PredictedAwayTeamScore  
            };
        }

        public static Prediction ToPredictionFromCreateDTO(CreatePredictionDto predictionDto)
        {
            return new Prediction
            {
                UserId = predictionDto.UserId,
                GameId = predictionDto.GameId,
                PredictedTeamId = predictionDto.PredictedTeamId,
                PredictedAwayTeamScore = predictionDto.PredictedAwayTeamScore,
                PredictedHomeTeamScore = predictionDto.PredictedHomeTeamScore,
                PredictionDate = DateOnly.FromDateTime(DateTime.UtcNow)
            };
        }
    }
}