using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Dtos.Team
{
    public class CreateTeamDto
    {
        public string City { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int SportId { get; set; }
        public string LogoURL { get; set; } = string.Empty;
    }
}