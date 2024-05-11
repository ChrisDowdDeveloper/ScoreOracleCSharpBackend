using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Injury;

namespace ScoreOracleCSharp.Dtos.Player
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public bool IsInjured { get; set; }
        public List<InjuryDto> PlayerInjury { get; set; }
    }
}