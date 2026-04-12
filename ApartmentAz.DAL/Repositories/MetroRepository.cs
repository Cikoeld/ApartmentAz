using ApartmentAz.DAL.Data;
using ApartmentAz.DAL.Interfaces;
using ApartmentAz.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Repositories
{
    public class MetroRepository : IMetroRepository
    {
        private readonly AppDbContext _context;

        public MetroRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Metro>> GetByCityAsync(Guid cityId)
        {
            return await _context.Metros
                .Include(x => x.Translations)
                .Where(x => x.CityId == cityId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
