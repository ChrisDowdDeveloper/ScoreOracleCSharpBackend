using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Dtos.Injury
{
    public class CreateInjuryDto
    {
        public int PlayerId { get; set; }
        public string Description { get; set; } = string.Empty;
        public int TeamId { get; set; }
    }
}