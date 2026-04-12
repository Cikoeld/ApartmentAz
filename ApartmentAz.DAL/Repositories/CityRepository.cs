using ApartmentAz.DAL.Data;
using ApartmentAz.DAL.Interfaces;
using ApartmentAz.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly AppDbContext _context;

        public CityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<City>> GetAllAsync()
        {
            return await _context.Cities
                .Include(x => x.Translations)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
