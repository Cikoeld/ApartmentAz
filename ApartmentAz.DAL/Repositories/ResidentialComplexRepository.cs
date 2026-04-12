using ApartmentAz.DAL.Data;
using ApartmentAz.DAL.Interfaces;
using ApartmentAz.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Repositories
{
    public class ResidentialComplexRepository : IResidentialComplexRepository
    {
        private readonly AppDbContext _context;

        public ResidentialComplexRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ResidentialComplex>> GetAllAsync()
        {
            return await _context.ResidentialComplexes
                .Include(x => x.Translations)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ResidentialComplex?> GetByIdAsync(Guid id)
        {
            return await _context.ResidentialComplexes
                .Include(x => x.Translations)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
