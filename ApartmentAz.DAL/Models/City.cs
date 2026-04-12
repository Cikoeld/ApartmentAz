using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

namespace ApartmentAz.DAL.Models
{
    public class City
    {
        public Guid Id { get; set; }

        public ICollection<CityTranslation> Translations { get; set; }

        public ICollection<District> Districts { get; set; }
        public ICollection<Metro> Metros { get; set; }
    }
}
