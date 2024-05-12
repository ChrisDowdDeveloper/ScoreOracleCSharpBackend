using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Helpers
{
    public class GroupMemberQueryObject
    {
        public string? GroupName { get; set; }
        public string? UserName { get; set; }
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; }
    }
}