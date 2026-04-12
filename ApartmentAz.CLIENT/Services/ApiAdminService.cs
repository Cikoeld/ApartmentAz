using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ApartmentAz.CLIENT.Services;

public class ApiAdminService
{
    private readonly HttpClient _http;

    public ApiAdminService(HttpClient http) => _http = http;

    // ── Listings ─────────────────────────────────────────────────────────

    public async Task<List<AdminListingApiModel>> GetListingsAsync(string token, string lang = "az")
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"api/admin/listings?lang={lang}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _http.SendAsync(request);
        if (!response.IsSuccessStatusCode) return [];
        return await response.Content.ReadFromJsonAsync<List<AdminListingApiModel>>() ?? [];
    }

    public async Task<bool> ApproveListingAsync(Guid id, string token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, $"api/admin/listings/{id}/approve");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _http.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RejectListingAsync(Guid id, string token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, $"api/admin/listings/{id}/reject");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _http.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateListingAsync(Guid id, object dto, string token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, $"api/admin/listings/{id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(dto);
        var response = await _http.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteListingAsync(Guid id, string token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"api/admin/listings/{id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _http.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    // ── Users ────────────────────────────────────────────────────────────

    public async Task<List<AdminUserApiModel>> GetUsersAsync(string token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "api/admin/users");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _http.SendAsync(request);
        if (!response.IsSuccessStatusCode) return [];
        return await response.Content.ReadFromJsonAsync<List<AdminUserApiModel>>() ?? [];
    }

    public async Task<bool> BanUserAsync(Guid id, string token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, $"api/admin/users/{id}/ban");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _http.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UnbanUserAsync(Guid id, string token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, $"api/admin/users/{id}/unban");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _http.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateUserAsync(Guid id, object dto, string token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, $"api/admin/users/{id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(dto);
        var response = await _http.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteUserAsync(Guid id, string token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"api/admin/users/{id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _http.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    // ── Dashboard ────────────────────────────────────────────────────────

    public async Task<DashboardStatsApiModel?> GetDashboardStatsAsync(string token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "api/admin/stats");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _http.SendAsync(request);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<DashboardStatsApiModel>();
    }

    public async Task<DashboardFullApiModel?> GetFullDashboardAsync(string token, string lang = "az")
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"api/admin/dashboard?lang={lang}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _http.SendAsync(request);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<DashboardFullApiModel>();
    }
}

public class AdminListingApiModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int RoomCount { get; set; }
    public double Area { get; set; }
    public int Floor { get; set; }
    public int TotalFloors { get; set; }
    public bool IsApproved { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CityName { get; set; }
    public string OwnerEmail { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
}

public class AdminUserApiModel
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool IsBanned { get; set; }
    public List<string> Roles { get; set; } = [];
}

public class DashboardStatsApiModel
{
    public int TotalListings { get; set; }
    public int PendingListings { get; set; }
    public int ApprovedListings { get; set; }
    public int TotalUsers { get; set; }
    public int BannedUsers { get; set; }
}

public class DashboardChartDataApiModel
{
    public List<string> ListingsPerDayLabels { get; set; } = [];
    public List<int> ListingsPerDayValues { get; set; } = [];
    public List<string> RegistrationsPerMonthLabels { get; set; } = [];
    public List<int> RegistrationsPerMonthValues { get; set; } = [];
}

public class RecentPendingListingApiModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? CityName { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class RecentUserApiModel
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class DashboardFullApiModel
{
    public int TotalListings { get; set; }
    public int PendingListings { get; set; }
    public int ApprovedListings { get; set; }
    public int TotalUsers { get; set; }
    public int BannedUsers { get; set; }
    public DashboardChartDataApiModel ChartData { get; set; } = new();
    public List<RecentPendingListingApiModel> RecentPendingListings { get; set; } = [];
    public List<RecentUserApiModel> RecentUsers { get; set; } = [];
}
