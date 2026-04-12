using ApartmentAz.BLL.DTOs.Location;
using ApartmentAz.BLL.Interfaces;
using ApartmentAz.BLL.Models;
using ApartmentAz.DAL.Interfaces;

namespace ApartmentAz.BLL.Services;

public class DistrictService : IDistrictService
{
    private readonly IDistrictRepository _districtRepository;

    public DistrictService(IDistrictRepository districtRepository)
    {
        _districtRepository = districtRepository;
    }

    public async Task<Result<List<DistrictDto>>> GetByCityAsync(Guid cityId, string lang)
    {
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

        return Result<List<DistrictDto>>.Success(result);
    }
}
