using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Group;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Mappers
{
    public static class GroupMapper
    {
        public static GroupDto ToGroupDto(this Group groupModel)
        {
            return new GroupDto
            {
                Id = groupModel.Id,
                Name = groupModel.Name,
                CreatedByUserId = groupModel.CreatedByUserId,
                CreatedOn = groupModel.CreatedOn,
                MemberCount = groupModel.Members.Count,
                Members = groupModel.Members.Select(m => GroupMemberMapper.ToGroupMemberDto(m)).ToList()
            };
        }

        public static Group ToGroupFromCreateDTO(CreateGroupDto groupDto)
        {
            return new Group
            {
                Name = groupDto.Name,
                CreatedByUserId = groupDto.CreatedByUserId,
                CreatedOn = DateTime.UtcNow
            };
        }
    }
}