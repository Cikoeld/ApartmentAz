using ApartmentAz.BLL.DTOs.ResidentialComplex;
using ApartmentAz.BLL.Interfaces;
using ApartmentAz.BLL.Models;
using ApartmentAz.DAL.Interfaces;

namespace ApartmentAz.BLL.Services;

public class ResidentialComplexService : IResidentialComplexService
{
    private readonly IResidentialComplexRepository _repository;

    public ResidentialComplexService(IResidentialComplexRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<ResidentialComplexDto>>> GetAllAsync(string lang)
    {
        var complexes = await _repository.GetAllAsync();

        var result = complexes.Select(rc => new ResidentialComplexDto
        {
            Id = rc.Id,
            Name = rc.Translations?.FirstOrDefault(t => t.LanguageCode == lang)?.Name
                   ?? rc.Translations?.FirstOrDefault(t => t.LanguageCode == "az")?.Name
                   ?? string.Empty
        }).ToList();

        return Result<List<ResidentialComplexDto>>.Success(result);
    }

    public async Task<Result<ResidentialComplexDto>> GetByIdAsync(Guid id, string lang)
    {
        var complex = await _repository.GetByIdAsync(id);

        if (complex == null)
            return Result<ResidentialComplexDto>.Failure("Residential complex not found.");

        var dto = new ResidentialComplexDto
        {
            Id = complex.Id,
            Name = complex.Translations?.FirstOrDefault(t => t.LanguageCode == lang)?.Name
                   ?? complex.Translations?.FirstOrDefault(t => t.LanguageCode == "az")?.Name
                   ?? string.Empty
        };

        return Result<ResidentialComplexDto>.Success(dto);
    }
}
