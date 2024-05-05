using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Dtos.Game
{
    public class CreateGameDto
    {
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public DateOnly GameDate { get; set; }
        public int SportId { get; set; }

    }
}