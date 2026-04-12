using ApartmentAz.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Interfaces
{
    public interface IResidentialComplexRepository
    {
        Task<List<ResidentialComplex>> GetAllAsync();
        Task<ResidentialComplex?> GetByIdAsync(Guid id);
    }
}
