using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Dtos.Sport
{
    public class CreateSportDto
    {
        public string Name { get; set; } = string.Empty;
        public string League { get; set; } = string.Empty;
        public string LogoURL { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
    }
}