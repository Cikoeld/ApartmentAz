using ApartmentAz.CLIENT.ViewModels.Listing;
using ApartmentAz.CLIENT.ViewModels.Location;
using System.Text.Json.Serialization;

namespace ApartmentAz.CLIENT.Services;

public class ApiListingService
{
    private readonly HttpClient _http;

    public ApiListingService(HttpClient http) => _http = http;

    public async Task<PagedApiResult<ListingCardViewModel>> GetAllAsync(ListingFilterViewModel filter, string lang = "az")
    {
        // Use InvariantCulture for all numeric params to prevent comma decimal separators
        // from corrupting the query string on non-English locale servers (e.g. az-AZ, tr-TR)
        var q = new System.Text.StringBuilder();
        q.Append("api/listings?lang=").Append(Uri.EscapeDataString(lang));

        if (filter.CityId.HasValue)       q.Append("&cityId=").Append(filter.CityId);
        if (filter.DistrictId.HasValue)   q.Append("&districtId=").Append(filter.DistrictId);
        if (filter.MetroId.HasValue)      q.Append("&metroId=").Append(filter.MetroId);
        if (filter.MinPrice.HasValue)     q.Append("&minPrice=").Append(filter.MinPrice.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        if (filter.MaxPrice.HasValue)     q.Append("&maxPrice=").Append(filter.MaxPrice.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        if (filter.RoomCount.HasValue)    q.Append("&roomCount=").Append(filter.RoomCount.Value);
        if (filter.ListingType.HasValue)  q.Append("&listingType=").Append(filter.ListingType.Value);
        if (filter.PropertyType.HasValue) q.Append("&propertyType=").Append(filter.PropertyType.Value);
        if (filter.RepairStatus.HasValue) q.Append("&repairStatus=").Append(filter.RepairStatus.Value);
        if (filter.MinArea.HasValue)      q.Append("&minArea=").Append(filter.MinArea.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        if (filter.MaxArea.HasValue)      q.Append("&maxArea=").Append(filter.MaxArea.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        if (!string.IsNullOrWhiteSpace(filter.SortBy))
            q.Append("&sortBy=").Append(Uri.EscapeDataString(filter.SortBy.Trim()));
        q.Append("&pageNumber=").Append(filter.PageNumber);
        q.Append("&pageSize=").Append(filter.PageSize);

        var result = await _http.GetFromJsonAsync<PagedApiResult<ListingCardViewModel>>(q.ToString())
                     ?? new PagedApiResult<ListingCardViewModel>();

        foreach (var item in result.Items)
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

/// <summary>Mirrors BLL's PagedResult&lt;T&gt; for JSON deserialization in CLIENT.</summary>
public class PagedApiResult<T>
{
    [JsonPropertyName("items")]      public List<T> Items      { get; set; } = [];
    [JsonPropertyName("totalCount")] public int TotalCount     { get; set; }
    [JsonPropertyName("pageNumber")] public int PageNumber     { get; set; }
    [JsonPropertyName("pageSize")]   public int PageSize       { get; set; }
    [JsonPropertyName("totalPages")] public int TotalPages     { get; set; }
    [JsonPropertyName("hasPreviousPage")] public bool HasPreviousPage { get; set; }
    [JsonPropertyName("hasNextPage")]     public bool HasNextPage     { get; set; }
}
