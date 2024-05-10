using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Prediction;
using ScoreOracleCSharp.Helpers;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Interfaces
{
    public interface IPredictionRepository
    {
        Task<List<Prediction>> GetAllAsync(PredictionQueryObject query);
        Task<Prediction?> GetByIdAsync(int id);
        Task<Prediction> CreateAsync(Prediction predictionModel);
        Task<Prediction?> UpdateAsync(int id, UpdatePredictionDto predictionDto);
        Task<Prediction?> DeleteAsync(int id);
        Task<bool> UserExists(string userId);
        Task<bool> GameExists(int gameId);
        Task<bool> TeamExists(int teamId);
        Task<bool> UserCanModifyPrediction(string userId, int id);
    }
}