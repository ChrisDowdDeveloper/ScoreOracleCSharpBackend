using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScoreOracleCSharp.Dtos.Player;
using ScoreOracleCSharp.Mappers;

namespace ScoreOracleCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public PlayerController(ApplicationDBContext context)
        {
            _context = context;
        }

        // Get All Players
        [HttpGet]
        public IActionResult GetAll() 
        {
            var players = _context.Players.ToList();
        
            return Ok(players);
        }

        // Get Player By ID
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var player = _context.Players.Find(id);

            if(player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        // Create Player
        public IActionResult Create([FromBody] CreatePlayerDto playerDto)
        {

            if (string.IsNullOrWhiteSpace(playerDto.FirstName) || string.IsNullOrWhiteSpace(playerDto.LastName))
            {
                return BadRequest("Player name cannot be empty.");
            }

            if(!TeamExists(playerDto.TeamId))
            {
                return BadRequest("Team does not exist with that ID");
            }

            var newPlayer = PlayerMapper.ToPlayerFromCreateDTO(playerDto);
            _context.Players.Add(newPlayer);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = newPlayer.Id }, PlayerMapper.ToPlayerDto(newPlayer));
        }

        public bool TeamExists(int teamId)
        {
            return _context.Teams.Any(t => t.Id == teamId);
        }
    }
}