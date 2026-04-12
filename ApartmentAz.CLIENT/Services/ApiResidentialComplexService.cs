using ApartmentAz.CLIENT.ViewModels.Location;

namespace ApartmentAz.CLIENT.Services;

public class ApiResidentialComplexService
{
    private readonly HttpClient _http;

    public ApiResidentialComplexService(HttpClient http) => _http = http;

    public async Task<List<SelectItemViewModel>> GetAllAsync(string lang = "az")
    {
        return await _http.GetFromJsonAsync<List<SelectItemViewModel>>($"api/residentialcomplexes?lang={lang}") ?? [];
    }
}
