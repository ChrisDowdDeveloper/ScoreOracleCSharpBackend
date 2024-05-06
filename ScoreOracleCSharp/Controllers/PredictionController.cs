using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Prediction;
using ScoreOracleCSharp.Mappers;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictionController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public PredictionController(ApplicationDBContext context)
        {
            _context = context;
        }

        // Get All Predictions
        [HttpGet]
        public IActionResult GetAll() 
        {
            var predictions = _context.Predictions.ToList();
        
            return Ok(predictions);
        }

        // Get Predictions By ID
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var prediction = _context.Predictions.Find(id);

            if(prediction == null)
            {
                return NotFound();
            }

            return Ok(prediction);
        }

        // Create Prediction
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePredictionDto predictionDto)
        {
            if (predictionDto.UserId != GetAuthenticatedUserId())
            {
                return Unauthorized("You are not authorized to make predictions for other users.");
            }
            
            if(!await UserExists(predictionDto.UserId))
            {
                return BadRequest("User does not exist with that ID");
            }
            
            if(!await GameExists(predictionDto.GameId))
            {
                return BadRequest("Game does not exist with that ID");
            }

            if(!await TeamExists(predictionDto.PredictedTeamId))
            {
                return BadRequest("Team does not exist with that ID");
            }

            var newPrediction = PredictionMapper.ToPredictionFromCreateDTO(predictionDto);
            _context.Predictions.Add(newPrediction);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = newPrediction.Id }, PredictionMapper.ToPredictionDto(newPrediction));
        }

        //Update Prediction
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePredictionDto predictionDto)
        {

            if (predictionDto.UserId != GetAuthenticatedUserId())
            {
                return Unauthorized("You are not authorized to make predictions for other users.");
            }

            var prediction = await _context.Predictions.FindAsync(id);
            var game = await _context.Games.FindAsync(predictionDto.GameId);
            if(prediction == null)
            {
                return NotFound("Prediction not found.");
            }

            if(game == null)
            {
                return BadRequest("Invalid game.");
            } 
            else if(predictionDto.GameId.HasValue)
            {
                prediction.GameId = predictionDto.GameId.Value;
            }

            if(predictionDto.PredictedTeamId.HasValue && !await TeamExists(predictionDto.PredictedTeamId.Value))
            {
                return BadRequest("Invalid team.");
            }
            else if(predictionDto.PredictedTeamId.HasValue)
            {
                prediction.PredictedTeamId = predictionDto.PredictedTeamId;
            }

            if(predictionDto.PredictionDate > game.GameDate)
            {
                return BadRequest("Prediction date cannot be after the game has been played.");
            }
            prediction.PredictionDate = predictionDto.PredictionDate;
            
            prediction.PredictedAwayTeamScore = predictionDto.PredictedAwayTeamScore ?? 0;
            prediction.PredictedHomeTeamScore = predictionDto.PredictedHomeTeamScore ?? 0;

            await _context.SaveChangesAsync();
            return Ok(PredictionMapper.ToPredictionDto(prediction));
        }

        private async Task<bool> UserExists(int userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }

        private async Task<bool> GameExists(int gameId)
        {
            return await _context.Games.AnyAsync(g => g.Id == gameId);
        }

        private async Task<bool> TeamExists(int teamId)
        {
            return await _context.Teams.AnyAsync(t => t.Id == teamId);
        }

        private int GetAuthenticatedUserId()
        {
            return 0;
        }
    }
}