using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreOracleCSharp.Dtos.GroupMember
{
    public class CreateGroupMemberDto
    {
        public int GroupId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}