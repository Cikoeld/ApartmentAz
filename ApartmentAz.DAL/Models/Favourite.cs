using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Models
{
    public class Favorite
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        public Guid ListingId { get; set; }
        public Listing Listing { get; set; }
    }
}
