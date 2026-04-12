using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Models
{
    public class CityTranslation
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string LanguageCode { get; set; }

        public Guid CityId { get; set; }
    }
}
