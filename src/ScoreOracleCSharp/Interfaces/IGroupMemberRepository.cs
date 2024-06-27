using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreOracleCSharp.Helpers;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Interfaces
{
    public interface IGroupMemberRepository
    {
        Task<List<GroupMember>> GetAllAsync(GroupMemberQueryObject query);
        Task<GroupMember?> GetByIdAsync(int id);
        Task<GroupMember> CreateAsync(GroupMember groupMemberModel);
        Task<GroupMember?> DeleteAsync(int id);
        Task<bool> UserExists(string userId);
        Task<bool> UserCanModifyGroup(int groupId, string userId);
        Task<bool> GroupExists(int id);
    }
}