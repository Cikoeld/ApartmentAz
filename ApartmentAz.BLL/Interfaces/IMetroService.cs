using ApartmentAz.BLL.DTOs.Location;
using ApartmentAz.BLL.Models;

namespace ApartmentAz.BLL.Interfaces;

public interface IMetroService
{
    Task<Result<List<MetroDto>>> GetByCityAsync(Guid cityId, string lang);
}
