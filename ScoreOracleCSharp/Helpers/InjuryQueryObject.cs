using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Helpers
{
    public class InjuryQueryObject
    {
        public string? PlayerName { get; set; }
        public string? TeamName { get; set; }
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; }
    }
}