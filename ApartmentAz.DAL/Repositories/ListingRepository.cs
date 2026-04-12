using ApartmentAz.DAL.Data;
using ApartmentAz.DAL.Interfaces;
using ApartmentAz.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ApartmentAz.DAL.Repositories
{
    public class ListingRepository : IListingRepository
    {
        private readonly AppDbContext _context;

        public ListingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Listing listing)
        {
            await _context.Listings.AddAsync(listing);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Listing>> GetAllFilteredAsync(
            Expression<Func<Listing, bool>>? filter = null,
            Func<IQueryable<Listing>, IOrderedQueryable<Listing>>? orderBy = null)
        {
            IQueryable<Listing> query = _context.Listings
                .Include(x => x.Translations)
                .Include(x => x.Images)
                .Include(x => x.City).ThenInclude(c => c.Translations)
                .Include(x => x.District!).ThenInclude(d => d!.Translations)
                .AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return await query.ToListAsync();
        }

        public async Task<Listing?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Listings
                .Include(x => x.Translations)
                .Include(x => x.Images)
                .Include(x => x.City).ThenInclude(c => c.Translations)
                .Include(x => x.District!).ThenInclude(d => d!.Translations)
                .Include(x => x.Metro!).ThenInclude(m => m!.Translations)
                .Include(x => x.User)
                .Include(x => x.Agency)
                .Include(x => x.ResidentialComplex!).ThenInclude(rc => rc!.Translations)
                .Include(x => x.CommercialDetail)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Listing?> GetByIdAsync(Guid id)
        {
            return await _context.Listings
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task DeleteAsync(Listing listing)
        {
            _context.Listings.Remove(listing);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
