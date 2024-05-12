using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScoreOracleCSharp.Dtos.Friendship;
using ScoreOracleCSharp.Helpers;
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

        public async Task<List<Friendship>> GetAllAsync(FriendshipQueryObject query)
        {
            // Receiver Name & Status
            var friendships = _context.Friendships.Include(f => f.Receiver).AsQueryable();
            if(!string.IsNullOrWhiteSpace(query.ReceiverName))
            {
                friendships = friendships.Where(f => f.Receiver != null && (f.Receiver.FirstName + " " + f.Receiver.LastName).ToLower().Contains(query.ReceiverName));
            }

            if(!string.IsNullOrWhiteSpace(query.Status))
            {
                var typeStr = query.Status.ToUpper();
                if(Enum.TryParse<FriendshipStatus>(query.Status, out var statusEnum))
                {
                    friendships = friendships.Where(f => f.Status == statusEnum);
                }
                else
                {
                    throw new ArgumentException("Invalid friendship type specified");
                }
            }

            if(!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if(query.SortBy.Equals("ReceiverName",  StringComparison.OrdinalIgnoreCase))
                {
                    friendships = query.IsDescending 
                            ? friendships.OrderByDescending(f => 
                                f.Receiver != null ? (f.Receiver.FirstName + " " + f.Receiver.LastName).ToLower() : "") 
                            : friendships.OrderBy(f => 
                                f.Receiver != null ? (f.Receiver.FirstName + " " + f.Receiver.LastName).ToLower() : "");
                }

                if(query.SortBy.Equals("Status", StringComparison.OrdinalIgnoreCase))
                {
                    friendships = query.IsDescending 
                            ? friendships.OrderByDescending(f => 
                                f.Status) 
                            : friendships.OrderBy(f => 
                                f.Status);
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await friendships
                        .Skip(skipNumber)
                        .Take(query.PageSize)
                        .ToListAsync();
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