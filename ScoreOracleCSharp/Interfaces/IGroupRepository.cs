using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Dtos.Group;
using ScoreOracleCSharp.Helpers;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Interfaces
{
    public interface IGroupRepository
    {
        Task<List<Group>> GetAllAsync(GroupQueryObject query);
        Task<Group?> GetByIdAsync(int id);
        Task<Group> CreateAsync(Group groupModel);
        Task<Group?> UpdateAsync(int id, UpdateGroupDto groupDto);
        Task<Group?> DeleteAsync(int id);
        Task<bool> UserCanModifyGroup(string userId, int id);
        Task<bool> UserExists(string userId);
    }
}