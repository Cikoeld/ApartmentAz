using ApartmentAz.BLL.DTOs.Location;
using ApartmentAz.BLL.Models;

namespace ApartmentAz.BLL.Interfaces;

public interface ICityService
{
    Task<Result<List<CityDto>>> GetAllAsync(string lang);
}
