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
        Task<List<Listing>> GetAllFilteredAsync(
            Expression<Func<Listing, bool>>? filter = null,
            Func<IQueryable<Listing>, IOrderedQueryable<Listing>>? orderBy = null);
        Task<Listing?> GetByIdWithDetailsAsync(Guid id);
        Task<Listing?> GetByIdAsync(Guid id);
        Task DeleteAsync(Listing listing);
        Task SaveChangesAsync();
    }
}
