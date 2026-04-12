using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Interfaces
{
    public interface IMetroRepository
    {
        Task<List<DAL.Models.Metro>> GetByCityAsync(Guid cityId);
    }
}
