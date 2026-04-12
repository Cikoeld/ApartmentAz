using ApartmentAz.BLL.DTOs.Agency;
using ApartmentAz.BLL.Models;

namespace ApartmentAz.BLL.Interfaces;

public interface IAgencyService
{
    Task<Result<List<AgencyDto>>> GetAllAsync();
    Task<Result<AgencyDto>> GetByIdAsync(Guid id);
    Task<Result<Guid>> CreateAsync(CreateAgencyDto dto, Guid userId);
}
