using ApartmentAz.DAL.Data;
using ApartmentAz.DAL.Interfaces;
using ApartmentAz.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Repositories
{
    public class DistrictRepository : IDistrictRepository
    {
        private readonly AppDbContext _context;

        public DistrictRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<District>> GetByCityAsync(Guid cityId)
        {
            return await _context.Districts
                .Include(x => x.Translations)
                .Where(x => x.CityId == cityId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
