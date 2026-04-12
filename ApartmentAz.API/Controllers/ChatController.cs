using ApartmentAz.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApartmentAz.API.Controllers;

[ApiController]
[Route("api/chat")]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetChatHistory([FromQuery] Guid otherUserId, [FromQuery] Guid listingId)
    {
        var userId = GetUserId();
        var result = await _chatService.GetChatHistoryAsync(userId, otherUserId, listingId);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(new { error = result.ErrorMessage });
    }

    [HttpGet("conversations")]
    public async Task<IActionResult> GetConversations()
    {
        var userId = GetUserId();
        var result = await _chatService.GetConversationsAsync(userId);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(new { error = result.ErrorMessage });
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(claim, out var id) ? id : Guid.Empty;
    }
}
