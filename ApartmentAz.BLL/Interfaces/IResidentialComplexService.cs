using ApartmentAz.BLL.DTOs.ResidentialComplex;
using ApartmentAz.BLL.Models;

namespace ApartmentAz.BLL.Interfaces;

public interface IResidentialComplexService
{
    Task<Result<List<ResidentialComplexDto>>> GetAllAsync(string lang);
    Task<Result<ResidentialComplexDto>> GetByIdAsync(Guid id, string lang);
}
