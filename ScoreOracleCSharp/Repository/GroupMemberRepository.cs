using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Repository
{
    public class GroupMemberRepository : IGroupMemberRepository
    {
        private readonly ApplicationDBContext _context;
        public GroupMemberRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<GroupMember> CreateAsync(GroupMember groupMemberModel)
        {
            await _context.GroupMembers.AddAsync(groupMemberModel);
            await _context.SaveChangesAsync();
            return groupMemberModel;
        }

        public async Task<GroupMember?> DeleteAsync(int id)
        {
            var groupMember = await _context.GroupMembers.FirstOrDefaultAsync(gm => gm.Id == id);
            if(groupMember == null)
            {
                return null;
            }
            _context.GroupMembers.Remove(groupMember);
            await _context.SaveChangesAsync();
            return groupMember;
        }

        public async Task<List<GroupMember>> GetAllAsync()
        {
            return await _context.GroupMembers.ToListAsync();
        }

        public async Task<GroupMember?> GetByIdAsync(int id)
        {
            return await _context.GroupMembers.FindAsync(id);
        }

        public async Task<bool> GroupExists(int id)
        {
            return await _context.Groups.AnyAsync(g => g.Id == id);
        }

        public async Task<bool> UserCanModifyGroup(int groupId, string userId)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group == null)
            {
                return false;
            }
            return userId == group.CreatedByUserId;
        }
        public async Task<bool> UserExists(string userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }
    }
}