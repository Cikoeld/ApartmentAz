using ApartmentAz.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Interfaces
{
    public interface IFavoriteRepository
    {
        Task AddAsync(Favorite favorite);
        Task RemoveAsync(Guid userId, Guid listingId);
        Task<List<Favorite>> GetUserFavoritesAsync(Guid userId);
        Task<bool> ExistsAsync(Guid userId, Guid listingId);
    }
}
