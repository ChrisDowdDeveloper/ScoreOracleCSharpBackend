using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Dtos.Game
{
    public class UpdateGameDto
    {
        public int? HomeTeamId { get; set; }
        public int? AwayTeamId { get; set; }
        public DateOnly? GameDate { get; set; }
        public GameStatus? GameStatus { get; set; }
        public int? SportId { get; set; }
    }
}