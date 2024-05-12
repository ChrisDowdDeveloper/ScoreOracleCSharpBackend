using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ScoreOracleCSharp.Helpers;
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

        public async Task<List<GroupMember>> GetAllAsync(GroupMemberQueryObject query)
        {
            var members = _context.GroupMembers
                                    .Include(gm => gm.Group)
                                    .Include(gm => gm.User)
                                    .AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.GroupName))
            {
                members = members.Where(gm => gm.Group != null && gm.Group.Name.Contains(query.GroupName));
            }

            if(!string.IsNullOrWhiteSpace(query.UserName))
            {
                members = members.Where(gm => gm.User != null && gm.User.UserName != null && gm.User.UserName.Contains(query.UserName));
            }

            if(!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if(query.SortBy.Equals("UserName", StringComparison.OrdinalIgnoreCase))
                {
                    members = query.IsDescending 
                        ? members.OrderByDescending(m => 
                            m.User != null ?  m.User.UserName : "") 
                        : members.OrderBy(m => 
                            m.User != null ?  m.User.UserName : "");
                }
                if(query.SortBy.Equals("GroupName", StringComparison.OrdinalIgnoreCase))
                {
                    members = query.IsDescending 
                            ? members.OrderByDescending(m => 
                                m.Group != null ? m.Group.Name : "")
                            : members.OrderBy(m => 
                                m.Group != null ? m.Group.Name : "");
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await members
                .Skip(skipNumber)
                .Take(query.PageSize)
                .ToListAsync();
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