using ApartmentAz.CLIENT.ViewModels.Listing;
using ApartmentAz.CLIENT.ViewModels.Location;

namespace ApartmentAz.CLIENT.Services;

public class ApiListingService
{
    private readonly HttpClient _http;

    public ApiListingService(HttpClient http) => _http = http;

    public async Task<List<ListingCardViewModel>> GetAllAsync(ListingFilterViewModel filter, string lang = "az")
    {
        var query = $"api/listings?lang={lang}";
        if (filter.CityId.HasValue) query += $"&cityId={filter.CityId}";
        if (filter.DistrictId.HasValue) query += $"&districtId={filter.DistrictId}";
        if (filter.MinPrice.HasValue) query += $"&minPrice={filter.MinPrice}";
        if (filter.MaxPrice.HasValue) query += $"&maxPrice={filter.MaxPrice}";
        if (filter.RoomCount.HasValue) query += $"&roomCount={filter.RoomCount}";
        if (filter.ListingType.HasValue) query += $"&listingType={filter.ListingType}";
        if (filter.PropertyType.HasValue) query += $"&propertyType={filter.PropertyType}";
        if (filter.MinArea.HasValue) query += $"&minArea={filter.MinArea}";
        if (filter.MaxArea.HasValue) query += $"&maxArea={filter.MaxArea}";
        if (filter.RepairStatus.HasValue) query += $"&repairStatus={filter.RepairStatus}";
        if (!string.IsNullOrEmpty(filter.SortBy)) query += $"&sortBy={filter.SortBy}";

        var result = await _http.GetFromJsonAsync<List<ListingCardViewModel>>(query) ?? [];
        foreach (var item in result)
            item.ThumbnailUrl = ResolveImageUrl(item.ThumbnailUrl);
        return result;
    }

    public async Task<ListingDetailsViewModel?> GetByIdAsync(Guid id, string lang = "az")
    {
        var vm = await _http.GetFromJsonAsync<ListingDetailsViewModel>($"api/listings/{id}?lang={lang}");
        if (vm != null)
            vm.ImageUrls = vm.ImageUrls.Select(ResolveImageUrl).ToList();
        return vm;
    }

    public async Task<HttpResponseMessage> CreateAsync(CreateListingViewModel vm, string token)
    {
        using var content = new MultipartFormDataContent();

        content.Add(new StringContent(vm.ListingType.ToString()), "ListingType");
        if (vm.RentType.HasValue)
            content.Add(new StringContent(vm.RentType.Value.ToString()), "RentType");
        content.Add(new StringContent(vm.PropertyType.ToString()), "PropertyType");
        content.Add(new StringContent(vm.SellerType.ToString()), "SellerType");
        content.Add(new StringContent(vm.RepairStatus.ToString()), "RepairStatus");
        content.Add(new StringContent(vm.Price.ToString()), "Price");
        content.Add(new StringContent(vm.RoomCount.ToString()), "RoomCount");
        content.Add(new StringContent(vm.Area.ToString()), "Area");
        content.Add(new StringContent(vm.Floor.ToString()), "Floor");
        content.Add(new StringContent(vm.TotalFloors.ToString()), "TotalFloors");
        content.Add(new StringContent(vm.HasDocument.ToString()), "HasDocument");
        content.Add(new StringContent(vm.HasMortgage.ToString()), "HasMortgage");
        content.Add(new StringContent(vm.Name    ?? string.Empty), "Name");
        content.Add(new StringContent(vm.Email   ?? string.Empty), "Email");
        content.Add(new StringContent(vm.Phone   ?? string.Empty), "Phone");
        content.Add(new StringContent(vm.CityId.ToString()), "CityId");
        if (vm.DistrictId.HasValue)
            content.Add(new StringContent(vm.DistrictId.Value.ToString()), "DistrictId");
        if (vm.MetroId.HasValue)
            content.Add(new StringContent(vm.MetroId.Value.ToString()), "MetroId");
        if (vm.AgencyId.HasValue)
            content.Add(new StringContent(vm.AgencyId.Value.ToString()), "AgencyId");
        if (vm.ResidentialComplexId.HasValue)
            content.Add(new StringContent(vm.ResidentialComplexId.Value.ToString()), "ResidentialComplexId");

        // Translations
        AddTranslation(content, 0, "az", vm.TitleAz, vm.DescriptionAz);
        AddTranslation(content, 1, "en", vm.TitleEn, vm.DescriptionEn);
        AddTranslation(content, 2, "ru", vm.TitleRu, vm.DescriptionRu);

        // Commercial detail (for PropertyType == 7)
        if (vm.PropertyType == 7)
        {
            content.Add(new StringContent(vm.HasParking.ToString()), "CommercialDetail.HasParking");
            content.Add(new StringContent(vm.HasHeating.ToString()), "CommercialDetail.HasHeating");
            content.Add(new StringContent(vm.HasAirConditioner.ToString()), "CommercialDetail.HasAirConditioner");
        }

        // Images
        foreach (var file in vm.Images)
        {
            var stream = file.OpenReadStream();
            content.Add(new StreamContent(stream), "Images", file.FileName);
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, "api/listings");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        request.Content = content;

        return await _http.SendAsync(request);
    }

    public async Task<HttpResponseMessage> DeleteAsync(Guid id, string token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"api/listings/{id}");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        return await _http.SendAsync(request);
    }

    private static void AddTranslation(MultipartFormDataContent content, int idx, string lang, string title, string desc)
    {
        content.Add(new StringContent(lang), $"Translations[{idx}].LanguageCode");
        content.Add(new StringContent(title ?? string.Empty), $"Translations[{idx}].Title");
        content.Add(new StringContent(desc ?? string.Empty), $"Translations[{idx}].Description");
    }

    private string ResolveImageUrl(string? url)
    {
        if (string.IsNullOrEmpty(url) || !url.StartsWith('/'))
            return url ?? string.Empty;
        var baseUrl = _http.BaseAddress?.ToString().TrimEnd('/') ?? string.Empty;
        return baseUrl + url;
    }
}
