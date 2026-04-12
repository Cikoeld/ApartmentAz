using ApartmentAz.DAL.Data;
using ApartmentAz.DAL.Interfaces;
using ApartmentAz.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ApartmentAz.DAL.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _context;

    public MessageRepository(AppDbContext context) => _context = context;

    public async Task<List<Message>> GetChatHistoryAsync(Guid senderId, Guid receiverId, Guid listingId, int take = 50)
    {
        return await _context.Messages
            .Where(m => m.ListingId == listingId &&
                ((m.SenderId == senderId && m.ReceiverId == receiverId) ||
                 (m.SenderId == receiverId && m.ReceiverId == senderId)))
            .OrderByDescending(m => m.CreatedAt)
            .Take(take)
            .OrderBy(m => m.CreatedAt)
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .ToListAsync();
    }

    public async Task<List<Message>> GetConversationsForUserAsync(Guid userId)
    {
        // Get the latest message per conversation (unique combo of other user + listing)
        var conversations = await _context.Messages
            .Where(m => m.SenderId == userId || m.ReceiverId == userId)
            .GroupBy(m => new
            {
                OtherUserId = m.SenderId == userId ? m.ReceiverId : m.SenderId,
                m.ListingId
            })
            .Select(g => g.OrderByDescending(m => m.CreatedAt).First())
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Include(m => m.Listing)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

        return conversations;
    }

    public async Task AddAsync(Message message)
    {
        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
