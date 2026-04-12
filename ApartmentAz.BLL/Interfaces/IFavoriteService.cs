using ApartmentAz.BLL.DTOs.Favorite;
using ApartmentAz.BLL.Models;

namespace ApartmentAz.BLL.Interfaces;

public interface IFavoriteService
{
    Task<Result<bool>> AddAsync(Guid userId, Guid listingId);
    Task<Result<bool>> RemoveAsync(Guid userId, Guid listingId);
    Task<Result<List<FavoriteDto>>> GetUserFavoritesAsync(Guid userId, string lang);
    Task<Result<bool>> ExistsAsync(Guid userId, Guid listingId);
}
