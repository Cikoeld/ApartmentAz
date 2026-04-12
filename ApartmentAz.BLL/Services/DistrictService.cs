using ApartmentAz.BLL.Caching;
using ApartmentAz.BLL.DTOs.Location;
using ApartmentAz.BLL.Interfaces;
using ApartmentAz.BLL.Models;
using ApartmentAz.DAL.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ApartmentAz.BLL.Services;

public class DistrictService : IDistrictService
{
    private readonly IDistrictRepository _districtRepository;
    private readonly IMemoryCache _cache;

    public DistrictService(IDistrictRepository districtRepository, IMemoryCache cache)
    {
        _districtRepository = districtRepository;
        _cache = cache;
    }

    public async Task<Result<List<DistrictDto>>> GetByCityAsync(Guid cityId, string lang)
    {
        var cacheKey = $"{CacheKeys.DistrictsPrefix}{cityId}_{lang}";

        if (_cache.TryGetValue(cacheKey, out List<DistrictDto>? cached) && cached != null)
            return Result<List<DistrictDto>>.Success(cached);

        var districts = await _districtRepository.GetByCityAsync(cityId);

        var result = districts.Select(d => new DistrictDto
        {
            Id = d.Id,
            Name = d.Translations?.FirstOrDefault(t => t.LanguageCode == lang)?.Name
                   ?? d.Translations?.FirstOrDefault(t => t.LanguageCode == "az")?.Name
                   ?? string.Empty
        })
        .OrderBy(d => d.Name)
        .ToList();

        CacheInvalidator.TrackKey(cacheKey);
        _cache.Set(cacheKey, result, TimeSpan.FromHours(1));

        return Result<List<DistrictDto>>.Success(result);
    }
}
