using ApartmentAz.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetByEmailAsync(string email);
        Task AddAsync(AppUser user);
    }
}
