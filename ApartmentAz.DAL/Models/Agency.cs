using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Models
{
    public class Agency
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }

        public Guid? UserId { get; set; } // owner
        public AppUser User { get; set; }

        public ICollection<Listing> Listings { get; set; }
    }
}
