using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Models
{
    public class MetroTranslation
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string LanguageCode { get; set; }

        public Guid MetroId { get; set; }
    }
}
