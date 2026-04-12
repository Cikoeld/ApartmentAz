using ApartmentAz.BLL.Caching;
using ApartmentAz.BLL.DTOs.Location;
using ApartmentAz.BLL.Interfaces;
using ApartmentAz.BLL.Models;
using ApartmentAz.DAL.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ApartmentAz.BLL.Services;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;
    private readonly IMemoryCache _cache;

    public CityService(ICityRepository cityRepository, IMemoryCache cache)
    {
        _cityRepository = cityRepository;
        _cache = cache;
    }

    public async Task<Result<List<CityDto>>> GetAllAsync(string lang)
    {
        var cacheKey = $"{CacheKeys.CitiesPrefix}{lang}";

        if (_cache.TryGetValue(cacheKey, out List<CityDto>? cached) && cached != null)
            return Result<List<CityDto>>.Success(cached);

        var cities = await _cityRepository.GetAllAsync();

        var result = cities.Select(c => new CityDto
        {
            Id = c.Id,
            Name = c.Translations?.FirstOrDefault(t => t.LanguageCode == lang)?.Name
                   ?? c.Translations?.FirstOrDefault(t => t.LanguageCode == "az")?.Name
                   ?? string.Empty
        })
        .OrderBy(c => c.Name)
        .ToList();

        CacheInvalidator.TrackKey(cacheKey);
        _cache.Set(cacheKey, result, TimeSpan.FromHours(1));

        return Result<List<CityDto>>.Success(result);
    }
}
