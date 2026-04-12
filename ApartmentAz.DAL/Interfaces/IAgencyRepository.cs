using ApartmentAz.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Interfaces
{
    public interface IAgencyRepository
    {
        Task<List<Agency>> GetAllAsync();
        Task<Agency?> GetByIdAsync(Guid id);
        Task CreateAsync(Agency agency);
    }
}
