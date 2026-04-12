using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Models
{
    public class ListingImage
    {
        public Guid Id { get; set; }

        public string ImageUrl { get; set; } // /images/listings/xxx.jpg

        public Guid ListingId { get; set; }
        public Listing Listing { get; set; }
    }
}
