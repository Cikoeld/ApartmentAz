using ApartmentAz.BLL.Caching;
using ApartmentAz.BLL.DTOs.Location;
using ApartmentAz.BLL.Interfaces;
using ApartmentAz.BLL.Models;
using ApartmentAz.DAL.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ApartmentAz.BLL.Services;

public class MetroService : IMetroService
{
    private readonly IMetroRepository _metroRepository;
    private readonly IMemoryCache _cache;

    public MetroService(IMetroRepository metroRepository, IMemoryCache cache)
    {
        _metroRepository = metroRepository;
        _cache = cache;
    }

    public async Task<Result<List<MetroDto>>> GetByCityAsync(Guid cityId, string lang)
    {
        var cacheKey = $"{CacheKeys.MetrosPrefix}{cityId}_{lang}";

        if (_cache.TryGetValue(cacheKey, out List<MetroDto>? cached) && cached != null)
            return Result<List<MetroDto>>.Success(cached);

        var metros = await _metroRepository.GetByCityAsync(cityId);

        var result = metros.Select(m => new MetroDto
        {
            Id = m.Id,
            Name = m.Translations?.FirstOrDefault(t => t.LanguageCode == lang)?.Name
                   ?? m.Translations?.FirstOrDefault(t => t.LanguageCode == "az")?.Name
                   ?? string.Empty
        })
        .OrderBy(m => m.Name)
        .ToList();

        CacheInvalidator.TrackKey(cacheKey);
        _cache.Set(cacheKey, result, TimeSpan.FromHours(1));

        return Result<List<MetroDto>>.Success(result);
    }
}
