using ApartmentAz.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ApartmentAz.DAL.Interfaces
{
    public interface IListingRepository
    {
        Task CreateAsync(Listing listing);

        /// <summary>
        /// Returns a bare, non-tracked IQueryable — no Includes.
        /// The service layer applies projections, filters and pagination
        /// so EF Core generates minimal SQL.
        /// </summary>
        IQueryable<Listing> GetQueryable();

        Task<List<Listing>> GetAllFilteredAsync(
            Expression<Func<Listing, bool>>? filter = null,
            Func<IQueryable<Listing>, IOrderedQueryable<Listing>>? orderBy = null);
        Task<Listing?> GetByIdWithDetailsAsync(Guid id);
        Task<Listing?> GetByIdAsync(Guid id);
        Task DeleteAsync(Listing listing);
        Task SaveChangesAsync();
    }
}
