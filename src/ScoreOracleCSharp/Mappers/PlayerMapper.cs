using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Player;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Mappers
{
    public static class PlayerMapper
    {
        public static PlayerDto ToPlayerDto(Player playerModel)
        {
            return new PlayerDto
            {
                Id = playerModel.Id,
                TeamId = playerModel.TeamId ?? 0,
                TeamName = playerModel.Team?.Name ?? "Unknown",
                FirstName = playerModel.FirstName,
                LastName = playerModel.LastName,
                Position = playerModel.Position,
                IsInjured = playerModel.PlayerInjury.Any(),
                PlayerInjury = playerModel.PlayerInjury.Select(i => InjuryMapper.ToInjuryDto(i)).ToList()
            };
        }

        public static Player ToPlayerFromCreateDTO(CreatePlayerDto playerDto)
        {
            return new Player
            {
                TeamId = playerDto.TeamId,
                FirstName = playerDto.FirstName,
                LastName = playerDto.LastName,
                Position = playerDto.Position
            };
        }
    }
}