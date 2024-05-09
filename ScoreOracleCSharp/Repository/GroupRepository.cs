using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Game;
using ScoreOracleCSharp.Dtos.Group;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Repository
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDBContext _context;

        public GroupRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Group> CreateAsync(Group groupModel)
        {
            await _context.Groups.AddAsync(groupModel);
            await _context.SaveChangesAsync();
            return groupModel;
        }

        public async Task<Group?> DeleteAsync(int id)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == id);
            if(group == null)
            {
                return null;
            }
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            return group;
        }

        public async Task<List<Group>> GetAllAsync()
        {
            return await _context.Groups.ToListAsync();
        }

        public async Task<Group?> GetByIdAsync(int id)
        {
            return await _context.Groups.FindAsync(id);
        }

        public async Task<Group?> UpdateAsync(int id, UpdateGroupDto groupDto)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == id);
            if(group == null)
            {
                return null;
            }
            group.Name = groupDto.Name;
            await _context.SaveChangesAsync();
            return group;
        }

        public async Task<bool> UserCanModifyGroup(string userId, int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if(group == null)
            {
                return false;
            }
            return group.CreatedByUserId == userId;
        }

        public async Task<bool> UserExists(string userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }
    }
}