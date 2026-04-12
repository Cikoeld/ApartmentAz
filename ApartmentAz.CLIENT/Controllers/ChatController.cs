using ApartmentAz.CLIENT.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApartmentAz.CLIENT.Controllers;

[Authorize]
public class ChatController : Controller
{
    private readonly ApiChatService _chatService;
    private readonly IConfiguration _configuration;

    public ChatController(ApiChatService chatService, IConfiguration configuration)
    {
        _chatService = chatService;
        _configuration = configuration;
    }

    public async Task<IActionResult> Index()
    {
        var token = GetToken();
        var conversations = await _chatService.GetConversationsAsync(token);
        return View(conversations);
    }

    public async Task<IActionResult> Room(Guid otherUserId, Guid listingId)
    {
        var token = GetToken();
        var history = await _chatService.GetChatHistoryAsync(token, otherUserId, listingId);

        ViewBag.OtherUserId = otherUserId;
        ViewBag.ListingId = listingId;
        ViewBag.CurrentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ViewBag.Token = token;
        ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"] ?? "https://localhost:7001";

        return View(history);
    }

    private string GetToken()
        => User.FindFirst("Token")?.Value ?? string.Empty;
}
