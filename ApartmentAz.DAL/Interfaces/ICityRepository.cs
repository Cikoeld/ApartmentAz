using ApartmentAz.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Interfaces
{
    public interface ICityRepository
    {
        Task<List<City>> GetAllAsync();
    }
}
