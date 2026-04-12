using ApartmentAz.BLL.DTOs.Location;
using ApartmentAz.BLL.Interfaces;
using ApartmentAz.BLL.Models;
using ApartmentAz.DAL.Interfaces;

namespace ApartmentAz.BLL.Services;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;

    public CityService(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<Result<List<CityDto>>> GetAllAsync(string lang)
    {
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

        return Result<List<CityDto>>.Success(result);
    }
}
