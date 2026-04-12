using ApartmentAz.BLL.DTOs.Listing;
using ApartmentAz.BLL.Models;

namespace ApartmentAz.BLL.Interfaces;

public interface IListingService
{
    Task<Result<Guid>> CreateAsync(CreateListingDto dto, Guid userId);
    Task<Result<List<ListingDto>>> GetAllAsync(ListingFilterDto filter);
    Task<Result<ListingDetailsDto>> GetByIdAsync(Guid id, string lang);
    Task<Result<bool>> DeleteAsync(Guid id, Guid userId);
}
