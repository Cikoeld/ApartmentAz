using ApartmentAz.BLL.DTOs.Chat;
using ApartmentAz.BLL.Interfaces;
using ApartmentAz.BLL.Models;
using ApartmentAz.DAL.Interfaces;
using ApartmentAz.DAL.Models;

namespace ApartmentAz.BLL.Services;

public class ChatService : IChatService
{
    private readonly IMessageRepository _messageRepository;

    public ChatService(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task<Result<ChatMessageDto>> SendMessageAsync(Guid senderId, SendMessageDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Content))
            return Result<ChatMessageDto>.Failure("Message content cannot be empty.");

        if (senderId == dto.ReceiverId)
            return Result<ChatMessageDto>.Failure("You cannot send a message to yourself.");

        var message = new Message
        {
            Id = Guid.NewGuid(),
            SenderId = senderId,
            ReceiverId = dto.ReceiverId,
            ListingId = dto.ListingId,
            Content = dto.Content.Trim(),
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };

        await _messageRepository.AddAsync(message);

        return Result<ChatMessageDto>.Success(new ChatMessageDto
        {
            Id = message.Id,
            SenderId = message.SenderId,
            SenderName = string.Empty, // Will be enriched by the hub/controller
            ReceiverId = message.ReceiverId,
            ReceiverName = string.Empty,
            ListingId = message.ListingId,
            Content = message.Content,
            CreatedAt = message.CreatedAt,
            IsRead = message.IsRead
        });
    }

    public async Task<Result<List<ChatMessageDto>>> GetChatHistoryAsync(Guid userId, Guid otherUserId, Guid listingId)
    {
        var messages = await _messageRepository.GetChatHistoryAsync(userId, otherUserId, listingId);

        var dtos = messages.Select(m => new ChatMessageDto
        {
            Id = m.Id,
            SenderId = m.SenderId,
            SenderName = m.Sender.FullName,
            ReceiverId = m.ReceiverId,
            ReceiverName = m.Receiver.FullName,
            ListingId = m.ListingId,
            Content = m.Content,
            CreatedAt = m.CreatedAt,
            IsRead = m.IsRead
        }).ToList();

        return Result<List<ChatMessageDto>>.Success(dtos);
    }

    public async Task<Result<List<ConversationDto>>> GetConversationsAsync(Guid userId)
    {
        var messages = await _messageRepository.GetConversationsForUserAsync(userId);

        var dtos = messages.Select(m =>
        {
            var isCurrentUserSender = m.SenderId == userId;
            return new ConversationDto
            {
                OtherUserId = isCurrentUserSender ? m.ReceiverId : m.SenderId,
                OtherUserName = isCurrentUserSender ? m.Receiver.FullName : m.Sender.FullName,
                ListingId = m.ListingId,
                LastMessage = m.Content.Length > 50 ? m.Content[..50] + "..." : m.Content,
                LastMessageAt = m.CreatedAt
            };
        }).ToList();

        return Result<List<ConversationDto>>.Success(dtos);
    }
}
