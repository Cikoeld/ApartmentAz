using ApartmentAz.BLL.DTOs.Chat;
using ApartmentAz.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ApartmentAz.API.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IChatService _chatService;

    public ChatHub(IChatService chatService)
    {
        _chatService = chatService;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();
        if (userId != Guid.Empty)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
        }
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(Guid receiverId, Guid listingId, string content)
    {
        var senderId = GetUserId();
        if (senderId == Guid.Empty) return;

        var dto = new SendMessageDto
        {
            ReceiverId = receiverId,
            ListingId = listingId,
            Content = content
        };

        var result = await _chatService.SendMessageAsync(senderId, dto);
        if (!result.IsSuccess) return;

        var message = result.Data!;
        message.SenderName = Context.User?.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";

        // Send to receiver
        await Clients.Group(receiverId.ToString()).SendAsync("ReceiveMessage", message);
        // Echo back to sender
        await Clients.Caller.SendAsync("ReceiveMessage", message);
    }

    private Guid GetUserId()
    {
        var claim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(claim, out var id) ? id : Guid.Empty;
    }
}
