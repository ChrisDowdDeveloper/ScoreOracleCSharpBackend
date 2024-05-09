using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Friendship;
using ScoreOracleCSharp.Interfaces;
using ScoreOracleCSharp.Models;

namespace ScoreOracleCSharp.Repository
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly ApplicationDBContext _context;

        public FriendshipRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Friendship> CreateAsync(Friendship friendshipModel)
        {
            await _context.Friendships.AddAsync(friendshipModel);
            await _context.SaveChangesAsync();
            return friendshipModel;
        }

        public async Task<Friendship?> DeleteAsync(int id)
        {
            var friendship = await _context.Friendships.FirstOrDefaultAsync(f => f.Id == id);
            if(friendship == null)
            {
                return null;
            }
            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();
            return friendship;
        }

        public async Task<List<Friendship>> GetAllAsync()
        {
            return await _context.Friendships.ToListAsync();
        }

        public async Task<Friendship?> GetByIdAsync(int id)
        {
            return await _context.Friendships.FindAsync(id);
        }

        public async Task<Friendship?> UpdateAsync(int id, UpdateFriendshipDto friendshipDto)
        {
            var friendship = await _context.Friendships.FirstOrDefaultAsync(f => f.Id == id);
            if(friendship == null)
            {
                return null;
            }
            friendship.Status = friendshipDto.Status;
            
            await _context.SaveChangesAsync();
            return friendship;
        }

        public async Task<bool> UserCanDeleteFriendship(string userId, int id)
        {
            var friendship = await _context.Friendships.FindAsync(id);
            if(friendship == null)
            {
                return false;
            }
            return friendship.RequesterId == userId || friendship.ReceiverId == userId;
        }

        public async Task<bool> UserExists(string userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }
    }
}