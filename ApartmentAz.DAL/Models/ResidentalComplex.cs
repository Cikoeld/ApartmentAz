using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Models
{
    public class ResidentialComplex
    {
        public Guid Id { get; set; }

        public ICollection<ResidentialComplexTranslation> Translations { get; set; }

        public ICollection<Listing> Listings { get; set; }
    }
}
