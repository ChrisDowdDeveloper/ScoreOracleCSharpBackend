using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Prediction;
using ScoreOracleCSharp.Helpers;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Mappers;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictionController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IPredictionRepository _predictionRepository;
        public PredictionController(ApplicationDBContext context, IPredictionRepository predictionRepository)
        {
            _predictionRepository = predictionRepository;
            _context = context;
        }

        /// <summary>
        /// Retrieves all predictions in the database.
        /// </summary>
        /// <returns>A list of predictions</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PredictionQueryObject query) 
        {
            var predictions = await _predictionRepository.GetAllAsync(query);
        
            return Ok(predictions);
        }

        /// <summary>
        /// Retrieves a prediction in the database.
        /// </summary>
        /// <returns>A specific prediction</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var prediction = await _predictionRepository.GetByIdAsync(id);

            if(prediction == null)
            {
                return NotFound();
            }

            return Ok(prediction);
        }

        /// <summary>
        /// Creates a prediction in the database
        /// </summary>
        /// <returns>The created prediction</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePredictionDto predictionDto)
        {
            if(!await _predictionRepository.UserExists(predictionDto.UserId))
            {
                return BadRequest("User does not exist with that ID");
            }
            
            if(!await _predictionRepository.GameExists(predictionDto.GameId))
            {
                return BadRequest("Game does not exist with that ID");
            }

            if(!await _predictionRepository.TeamExists(predictionDto.PredictedTeamId))
            {
                return BadRequest("Team does not exist with that ID");
            }

            var newPrediction = PredictionMapper.ToPredictionFromCreateDTO(predictionDto);
            var createdPrediction = await _predictionRepository.CreateAsync(newPrediction);
            return CreatedAtAction(nameof(GetById), new { id = newPrediction.Id }, PredictionMapper.ToPredictionDto(createdPrediction));
        }

        /// <summary>
        /// Updates a prediction in the database
        /// </summary>
        /// <returns>The updated prediction</returns>
        [HttpPatch]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePredictionDto predictionDto)
        {
            var userId = GetAuthenticatedUserId();
            if(!await _predictionRepository.UserCanModifyPrediction(userId, id))
            {
                return BadRequest("You cannot modify another persons prediction.");
            }
            try
            {
                var updatedPrediction = await _predictionRepository.UpdateAsync(id, predictionDto);
                if(updatedPrediction == null)
                {
                    return NotFound("Prediction not found");
                }
                return Ok(PredictionMapper.ToPredictionDto(updatedPrediction));
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a prediction in the database
        /// </summary>
        /// <returns>No Content</returns>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var userId = GetAuthenticatedUserId();
            if(!await _predictionRepository.UserCanModifyPrediction(userId, id))
            {
                return BadRequest("You cannot modify another persons prediction.");
            }
            await _predictionRepository.DeleteAsync(id);
            return NoContent();
        }
        private string GetAuthenticatedUserId()
        {
            return User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User must be authenticated.");
        }
    }
}