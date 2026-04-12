using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Models
{
    public class ListingTranslation
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public string LanguageCode { get; set; } // az, en, ru

        public Guid ListingId { get; set; }
        public Listing Listing { get; set; }
    }
}
