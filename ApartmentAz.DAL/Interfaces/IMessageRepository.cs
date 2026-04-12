using ApartmentAz.DAL.Models;

namespace ApartmentAz.DAL.Interfaces;

public interface IMessageRepository
{
    Task<List<Message>> GetChatHistoryAsync(Guid senderId, Guid receiverId, Guid listingId, int take = 50);
    Task<List<Message>> GetConversationsForUserAsync(Guid userId);
    Task AddAsync(Message message);
    Task SaveChangesAsync();
}
