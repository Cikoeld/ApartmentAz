using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ApartmentAz.CLIENT.Services;

public class ApiChatService
{
    private readonly HttpClient _http;

    public ApiChatService(HttpClient http) => _http = http;

    public async Task<List<ChatConversationApiModel>> GetConversationsAsync(string token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "api/chat/conversations");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _http.SendAsync(request);
        if (!response.IsSuccessStatusCode) return [];
        return await response.Content.ReadFromJsonAsync<List<ChatConversationApiModel>>() ?? [];
    }

    public async Task<List<ChatMessageApiModel>> GetChatHistoryAsync(string token, Guid otherUserId, Guid listingId)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get,
            $"api/chat/history?otherUserId={otherUserId}&listingId={listingId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _http.SendAsync(request);
        if (!response.IsSuccessStatusCode) return [];
        return await response.Content.ReadFromJsonAsync<List<ChatMessageApiModel>>() ?? [];
    }
}

public class ChatMessageApiModel
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public Guid ReceiverId { get; set; }
    public string ReceiverName { get; set; } = string.Empty;
    public Guid ListingId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
}

public class ChatConversationApiModel
{
    public Guid OtherUserId { get; set; }
    public string OtherUserName { get; set; } = string.Empty;
    public Guid ListingId { get; set; }
    public string LastMessage { get; set; } = string.Empty;
    public DateTime LastMessageAt { get; set; }
}
