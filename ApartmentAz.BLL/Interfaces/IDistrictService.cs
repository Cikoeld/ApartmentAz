using ApartmentAz.BLL.DTOs.Location;
using ApartmentAz.BLL.Models;

namespace ApartmentAz.BLL.Interfaces;

public interface IDistrictService
{
    Task<Result<List<DistrictDto>>> GetByCityAsync(Guid cityId, string lang);
}
