using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Sport;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Mappers
{
    public class SportMapper
    {
        public static SportDto ToSportDto(Sport sportModel)
        {
            return new SportDto
            {
                Id = sportModel.Id,
                Name = sportModel.Name,
                League = sportModel.League,
                LogoURL = sportModel.LogoURL,
                Abbreviation = sportModel.Abbreviation
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