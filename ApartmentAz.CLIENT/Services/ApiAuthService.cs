using System.Net.Http.Json;
using System.Text.Json;

namespace ApartmentAz.CLIENT.Services;

public class ApiAuthService
{
    private readonly HttpClient _http;

    private static readonly JsonSerializerOptions JsonOpts =
        new(JsonSerializerDefaults.Web);

    public ApiAuthService(HttpClient http) => _http = http;

    public async Task<AuthApiResult> RegisterAsync(object dto)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", dto);
        return await ReadResultAsync(response);
    }

    public async Task<AuthApiResult> LoginAsync(object dto)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", dto);
        return await ReadResultAsync(response);
    }

    private static async Task<AuthApiResult> ReadResultAsync(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();

        try
        {
            var result = JsonSerializer.Deserialize<AuthApiResult>(json, JsonOpts);
            if (result != null)
                return result;
        }
        catch { }

        return new AuthApiResult
        {
            Succeeded = false,
            ErrorMessage = response.IsSuccessStatusCode
                ? "Unexpected response format."
                : $"Request failed ({(int)response.StatusCode})."
        };
    }
}

public class AuthApiResult
{
    public bool Succeeded { get; set; }
    public string? Token { get; set; }
    public string? ErrorMessage { get; set; }
    public Guid? UserId { get; set; }
    public List<string> Roles { get; set; } = [];
}
