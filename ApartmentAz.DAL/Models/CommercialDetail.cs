using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Models
{
    public class CommercialDetail
    {
        public Guid Id { get; set; }

        public Guid ListingId { get; set; }
        public Listing Listing { get; set; }

        public bool HasParking { get; set; }
        public bool HasHeating { get; set; }
        public bool HasAirConditioner { get; set; }
    }
}
