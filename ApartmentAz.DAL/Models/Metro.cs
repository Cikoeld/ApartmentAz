using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Models
{
    public class Metro
    {
        public Guid Id { get; set; }

        public Guid CityId { get; set; }
        public City City { get; set; }

        public ICollection<MetroTranslation> Translations { get; set; }
    }
}
