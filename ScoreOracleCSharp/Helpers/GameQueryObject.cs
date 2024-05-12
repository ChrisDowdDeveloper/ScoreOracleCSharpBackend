using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Helpers
{
    public class GameQueryObject
    {
        public string? TeamName { get; set; }
        public DateOnly? GameDate { get; set; }
        public string? SportName { get; set; }
        public string? GameStatus { get; set; }
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}