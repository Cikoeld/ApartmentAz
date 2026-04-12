using ApartmentAz.BLL.DTOs.Chat;
using ApartmentAz.BLL.Models;

namespace ApartmentAz.BLL.Interfaces;

public interface IChatService
{
    Task<Result<ChatMessageDto>> SendMessageAsync(Guid senderId, SendMessageDto dto);
    Task<Result<List<ChatMessageDto>>> GetChatHistoryAsync(Guid userId, Guid otherUserId, Guid listingId);
    Task<Result<List<ConversationDto>>> GetConversationsAsync(Guid userId);
}
