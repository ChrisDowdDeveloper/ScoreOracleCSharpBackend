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
                CreatedByUserId = groupModel.UserId ?? 0,
                CreatedOn = groupModel.CreatedOn,
                MemberCount = groupModel.Members.Count
            };
        }

        public static Group ToGroupFromCreateDTO(CreateGroupDto groupDto)
        {
            return new Group
            {
                Name = groupDto.Name,
                UserId = groupDto.UserId,
                CreatedOn = DateTime.UtcNow
            };
        }
    }
}