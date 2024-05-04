using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.GroupMember;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Mappers
{
    public static class GroupMemberMapper
    {
        public static GroupMemberDto ToGroupMemberDto(this GroupMember groupMemberModel)
        {
            return new GroupMemberDto
            {
                Id = groupMemberModel.Id,
                GroupId = groupMemberModel.GroupId ?? 0,
                UserId = groupMemberModel.UserId ?? 0,
                Username = groupMemberModel.User?.Username ?? "Unknown",
                ProfilePictureUrl = groupMemberModel.User?.ProfilePictureUrl ?? string.Empty,
                JoinedAt = groupMemberModel.JoinedAt
            };
        }
    }
}