using ApartmentAz.CLIENT.ViewModels.Location;

namespace ApartmentAz.CLIENT.Services;

public class ApiLocationService
{
    private readonly HttpClient _http;

    public ApiLocationService(HttpClient http) => _http = http;

    public async Task<List<SelectItemViewModel>> GetCitiesAsync(string lang = "az")
    {
        return await _http.GetFromJsonAsync<List<SelectItemViewModel>>($"api/locations/cities?lang={lang}") ?? [];
    }

    public async Task<List<SelectItemViewModel>> GetDistrictsAsync(Guid cityId, string lang = "az")
    {
        return await _http.GetFromJsonAsync<List<SelectItemViewModel>>($"api/locations/districts?cityId={cityId}&lang={lang}") ?? [];
    }

    public async Task<List<SelectItemViewModel>> GetMetrosAsync(Guid cityId, string lang = "az")
    {
        return await _http.GetFromJsonAsync<List<SelectItemViewModel>>($"api/locations/metros?cityId={cityId}&lang={lang}") ?? [];
    }
}
