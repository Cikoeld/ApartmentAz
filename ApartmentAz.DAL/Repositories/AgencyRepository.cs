using ApartmentAz.DAL.Data;
using ApartmentAz.DAL.Interfaces;
using ApartmentAz.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Repositories
{
    public class AgencyRepository : IAgencyRepository
    {
        private readonly AppDbContext _context;

        public AgencyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Agency>> GetAllAsync()
        {
            return await _context.Agencies
                .Include(a => a.Listings)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Agency?> GetByIdAsync(Guid id)
        {
            return await _context.Agencies
                .Include(a => a.Listings)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task CreateAsync(Agency agency)
        {
            await _context.Agencies.AddAsync(agency);
            await _context.SaveChangesAsync();
        }
    }
}
