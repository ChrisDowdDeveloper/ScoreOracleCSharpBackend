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
                UserId = groupMemberModel.UserId,
                Username = groupMemberModel.User?.UserName ?? "Unknown",
                ProfilePictureUrl = groupMemberModel.User?.ProfilePictureUrl ?? string.Empty,
                JoinedAt = groupMemberModel.JoinedAt
            };
        }
        public static GroupMember ToGroupMemberFromCreateDTO(CreateGroupMemberDto memberDto)
        {
            return new GroupMember
            {
                GroupId = memberDto.GroupId,
                UserId = memberDto.UserId,
                JoinedAt = DateOnly.FromDateTime(DateTime.UtcNow)
            };
        }
    }
}