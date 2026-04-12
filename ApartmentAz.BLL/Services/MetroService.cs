using ApartmentAz.BLL.DTOs.Location;
using ApartmentAz.BLL.Interfaces;
using ApartmentAz.BLL.Models;
using ApartmentAz.DAL.Interfaces;

namespace ApartmentAz.BLL.Services;

public class MetroService : IMetroService
{
    private readonly IMetroRepository _metroRepository;

    public MetroService(IMetroRepository metroRepository)
    {
        _metroRepository = metroRepository;
    }

    public async Task<Result<List<MetroDto>>> GetByCityAsync(Guid cityId, string lang)
    {
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

        return Result<List<MetroDto>>.Success(result);
    }
}
