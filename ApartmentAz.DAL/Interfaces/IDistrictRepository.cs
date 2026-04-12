using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Interfaces
{
    public interface IDistrictRepository
    {
        Task<List<DAL.Models.District>> GetByCityAsync(Guid cityId);
    }
}
