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
                SportId = teamModel.SportId,
                SportName = teamModel.Sport?.Name ?? "Unknown",
                LogoURL = teamModel.LogoURL,
                HomeGames = teamModel.HomeGames.Select(h => GameMapper.ToGameDto(h)).ToList(),
                AwayGames = teamModel.AwayGames.Select(a => GameMapper.ToGameDto(a)).ToList(),
                InjuriesOnTeam = teamModel.InjuriesOnTeam.Select(i => InjuryMapper.ToInjuryDto(i)).ToList(),
                PlayersOnTeam = teamModel.PlayersOnTeam.Select(p => PlayerMapper.ToPlayerDto(p)).ToList(),
                TeamPredicted = teamModel.TeamPredicted.Select(p => PredictionMapper.ToPredictionDto(p)).ToList()
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