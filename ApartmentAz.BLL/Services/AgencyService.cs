using ApartmentAz.BLL.DTOs.Agency;
using ApartmentAz.BLL.Interfaces;
using ApartmentAz.BLL.Models;
using ApartmentAz.DAL.Interfaces;
using ApartmentAz.DAL.Models;
using AutoMapper;

namespace ApartmentAz.BLL.Services;

public class AgencyService : IAgencyService
{
    private readonly IAgencyRepository _agencyRepository;
    private readonly IMapper _mapper;

    public AgencyService(IAgencyRepository agencyRepository, IMapper mapper)
    {
        _agencyRepository = agencyRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<AgencyDto>>> GetAllAsync()
    {
        var agencies = await _agencyRepository.GetAllAsync();

        var result = agencies.Select(a => new AgencyDto
        {
            Id = a.Id,
            Name = a.Name,
            Phone = a.Phone,
            ListingCount = a.Listings?.Count ?? 0
        }).ToList();

        return Result<List<AgencyDto>>.Success(result);
    }

    public async Task<Result<AgencyDto>> GetByIdAsync(Guid id)
    {
        var agency = await _agencyRepository.GetByIdAsync(id);

        if (agency == null)
            return Result<AgencyDto>.Failure("Agency not found.");

        var dto = new AgencyDto
        {
            Id = agency.Id,
            Name = agency.Name,
            Phone = agency.Phone,
            ListingCount = agency.Listings?.Count ?? 0
        };

        return Result<AgencyDto>.Success(dto);
    }

    public async Task<Result<Guid>> CreateAsync(CreateAgencyDto dto, Guid userId)
    {
        var agency = _mapper.Map<Agency>(dto);
        agency.Id = Guid.NewGuid();
        agency.UserId = userId;

        await _agencyRepository.CreateAsync(agency);

        return Result<Guid>.Success(agency.Id);
    }
}
