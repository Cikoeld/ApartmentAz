using ApartmentAz.CLIENT.ViewModels.Location;

namespace ApartmentAz.CLIENT.Services;

public class ApiAgencyService
{
    private readonly HttpClient _http;

    public ApiAgencyService(HttpClient http) => _http = http;

    public async Task<List<SelectItemViewModel>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<List<SelectItemViewModel>>("api/agencies") ?? [];
    }
}
