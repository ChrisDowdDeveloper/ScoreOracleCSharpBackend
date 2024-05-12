using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Helpers
{
    public class SportQueryObject
    {
        public string? Name { get; set; } = null;
        public string? Abbreviation { get; set; } = null;
        public string? League { get; set; } = null;
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}