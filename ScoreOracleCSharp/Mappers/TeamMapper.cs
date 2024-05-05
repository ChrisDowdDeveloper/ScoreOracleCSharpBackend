using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Team;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Mappers
{
    public static class TeamMapper
    {
        public static TeamDto ToTeamDto(Team teamModel)
        {
            return new TeamDto
            {
                Id = teamModel.Id,
                City = teamModel.City,
                Name = teamModel.Name,
                SportId = teamModel.SportId ?? 0,
                SportName = teamModel.Sport?.Name ?? "Unknown",
                LogoURL = teamModel.LogoURL
            };
        }

        public static Team ToTeamFromCreateDTO(CreateTeamDto teamDto)
        {
            return new Team
            {
                City = teamDto.City,
                Name = teamDto.Name,
                SportId = teamDto.SportId,
                LogoURL = teamDto.LogoURL
            };
        }
    }
}