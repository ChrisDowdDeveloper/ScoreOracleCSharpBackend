using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ScoreOracleCSharp.Dtos.Injury;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Mappers
{
    public static class InjuryMapper
    {
        public static InjuryDto ToInjuryDto(Injury injuryModel)
        {
            return new InjuryDto
            {
                Id = injuryModel.Id,
                PlayerId = injuryModel.PlayerId ?? 0,
                PlayerName = injuryModel.Player?.FirstName + " " + injuryModel.Player?.LastName ?? "Unknown",
                TeamId = injuryModel.TeamId ?? 0,
                TeamName = injuryModel.Team?.Name ?? "Unknown",
                Description = injuryModel.Description
            };
        }
    }
}