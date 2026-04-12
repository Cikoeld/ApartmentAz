using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Repositories
{
    using ApartmentAz.DAL.Data;
    using ApartmentAz.DAL.Interfaces;
    using ApartmentAz.DAL.Models;
    using Microsoft.EntityFrameworkCore;

    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly AppDbContext _context;

        public FavoriteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Favorite favorite)
        {
            await _context.Favorites.AddAsync(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Guid userId, Guid listingId)
        {
            var entity = await _context.Favorites
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ListingId == listingId);

            if (entity != null)
            {
                _context.Favorites.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Favorite>> GetUserFavoritesAsync(Guid userId)
        {
            return await _context.Favorites
                .Include(x => x.Listing)
                    .ThenInclude(l => l.Images)
                .Include(x => x.Listing)
                    .ThenInclude(l => l.Translations)
                .Include(x => x.Listing)
                    .ThenInclude(l => l.City)
                        .ThenInclude(c => c.Translations)
                .Where(x => x.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(Guid userId, Guid listingId)
        {
            return await _context.Favorites
                .AnyAsync(x => x.UserId == userId && x.ListingId == listingId);
        }
    }
}
