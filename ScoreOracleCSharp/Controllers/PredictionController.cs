using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScoreOracleCSharp.Dtos.Prediction;
using ScoreOracleCSharp.Mappers;

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
        public IActionResult Create([FromBody] CreatePredictionDto predictionDto)
        {
            if (predictionDto.UserId != GetAuthenticatedUserId())
            {
                return Unauthorized("You are not authorized to make predictions for other users.");
            }
            
            if(!UserExists(predictionDto.UserId))
            {
                return BadRequest("User does not exist with that ID");
            }
            
            if(!GameExists(predictionDto.GameId))
            {
                return BadRequest("Game does not exist with that ID");
            }

            if(!TeamExists(predictionDto.PredictedTeamId))
            {
                return BadRequest("Team does not exist with that ID");
            }

            var newPrediction = PredictionMapper.ToPredictionFromCreateDTO(predictionDto);
            _context.Predictions.Add(newPrediction);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = newPrediction.Id }, PredictionMapper.ToPredictionDto(newPrediction));
        }

        private bool UserExists(int userId)
        {
            return _context.Users.Any(u => u.Id == userId);
        }

        private bool GameExists(int gameId)
        {
            return _context.Games.Any(g => g.Id == gameId);
        }

        private bool TeamExists(int teamId)
        {
            return _context.Teams.Any(t => t.Id == teamId);
        }

        private int GetAuthenticatedUserId()
        {
            return 0;
        }
    }
}