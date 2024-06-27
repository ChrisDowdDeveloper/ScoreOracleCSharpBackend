using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Sport;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Mappers
{
    public static class SportMapper
    {
        public static SportDto ToSportDto(Sport sportModel)
        {
            return new SportDto
            {
                Id = sportModel.Id,
                Name = sportModel.Name,
                League = sportModel.League,
                LogoURL = sportModel.LogoURL,
                Abbreviation = sportModel.Abbreviation,
                Teams = sportModel.Teams.Select(t => TeamMapper.ToTeamDto(t)).ToList(),
                Games = sportModel.Games.Select(g => GameMapper.ToGameDto(g)).ToList(),
                PlayersInSport = sportModel.PlayersInSport.Select(p => PlayerMapper.ToPlayerDto(p)).ToList(),
                LeaderboardsBySport = sportModel.LeaderboardsBySport.Select(l => LeaderboardMapper.ToLeaderboardDto(l)).ToList()
            };
        }

        public static Sport ToSportFromCreateDTO(CreateSportDto sportDto)
        {
            return new Sport
            {
                Name = sportDto.Name,
                League = sportDto.League,
                LogoURL = sportDto.LogoURL,
                Abbreviation = sportDto.Abbreviation
            };
        }
    }
}