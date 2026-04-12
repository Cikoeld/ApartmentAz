using ApartmentAz.CLIENT.ViewModels.Favorite;

namespace ApartmentAz.CLIENT.Services;

public class ApiFavoriteService
{
    private readonly HttpClient _http;

    public ApiFavoriteService(HttpClient http) => _http = http;

    public async Task<List<FavoriteViewModel>> GetAllAsync(string token, string lang = "az")
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"api/favorites?lang={lang}");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<List<FavoriteViewModel>>() ?? [];
        var baseUrl = _http.BaseAddress?.ToString().TrimEnd('/') ?? string.Empty;
        foreach (var item in result)
        {
            if (!string.IsNullOrEmpty(item.ThumbnailUrl) && item.ThumbnailUrl.StartsWith('/'))
                item.ThumbnailUrl = baseUrl + item.ThumbnailUrl;
        }
        return result;
    }

    public async Task<HttpResponseMessage> AddAsync(Guid listingId, string token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"api/favorites/{listingId}");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        return await _http.SendAsync(request);
    }

    public async Task<HttpResponseMessage> RemoveAsync(Guid listingId, string token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"api/favorites/{listingId}");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        return await _http.SendAsync(request);
    }
}
