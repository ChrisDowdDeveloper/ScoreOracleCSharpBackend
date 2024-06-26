using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.GroupMember;

namespace ScoreOracleCSharp.Dtos.Group
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CreatedByUserId { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public int MemberCount { get; set; }
        public List<GroupMemberDto> Members { get; set; }
    }
}