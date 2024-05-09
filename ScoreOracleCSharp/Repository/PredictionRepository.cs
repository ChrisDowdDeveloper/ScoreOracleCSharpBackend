using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Prediction;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Repository
{
    public class PredictionRepository : IPredictionRepository
    {

        private readonly ApplicationDBContext _context;
        public PredictionRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Prediction> CreateAsync(Prediction predictionModel)
        {
            await _context.Predictions.AddAsync(predictionModel);
            await _context.SaveChangesAsync();
            return predictionModel;
        }

        public async Task<Prediction?> DeleteAsync(int id)
        {
            var prediction = await _context.Predictions.FirstOrDefaultAsync(p => p.Id == id);
            if(prediction == null)
            {
                return null;
            }
            _context.Predictions.Remove(prediction);
            await _context.SaveChangesAsync();
            return prediction;
        }

        public async Task<bool> GameExists(int gameId)
        {
            return await _context.Games.AnyAsync(g => g.Id == gameId);
        }

        public async Task<List<Prediction>> GetAllAsync()
        {
            return await _context.Predictions.ToListAsync();
        }

        public async Task<Prediction?> GetByIdAsync(int id)
        {
            return await _context.Predictions.FindAsync(id);
        }

        public async Task<bool> TeamExists(int teamId)
        {
            return await _context.Teams.AnyAsync(t => t.Id == teamId);
        }

        public async Task<Prediction?> UpdateAsync(int id, UpdatePredictionDto predictionDto)
        {
            var prediction = await _context.Predictions.FirstOrDefaultAsync(p => p.Id == id);
            if(prediction == null)
            {
                return null;
            }
            if(predictionDto.GameId.HasValue)
            {
                prediction.GameId = predictionDto.GameId.Value;
            }

            if(predictionDto.PredictedTeamId.HasValue && !await TeamExists(predictionDto.PredictedTeamId.Value))
            {
                throw new Exception("Invalid team.");
            }
            else if(predictionDto.PredictedTeamId.HasValue)
            {
                prediction.PredictedTeamId = predictionDto.PredictedTeamId;
            }
            prediction.PredictionDate = predictionDto.PredictionDate;
            prediction.PredictedAwayTeamScore = predictionDto.PredictedAwayTeamScore ?? 0;
            prediction.PredictedHomeTeamScore = predictionDto.PredictedHomeTeamScore ?? 0;

            await _context.SaveChangesAsync();
            return prediction;
        }

        public Task<bool> UserCanModifyPrediction(string userId, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UserExists(string userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }
    }
}