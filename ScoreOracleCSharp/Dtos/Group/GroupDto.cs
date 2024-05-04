using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Dtos.Group
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CreatedByUserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int MemberCount { get; set; }
    }
}