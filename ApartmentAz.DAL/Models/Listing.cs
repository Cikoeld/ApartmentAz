using ApartmentAz.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

namespace ApartmentAz.DAL.Models
{
    public class Listing
    {
        public Guid Id { get; set; }

        // USER-DEFINED FIELDS
        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        // ENUM-lar
        public ListingType ListingType { get; set; }
        public RentType? RentType { get; set; } // yalnız Rent üçün
        public PropertyType PropertyType { get; set; }
        public SellerType SellerType { get; set; }
        public RepairStatus RepairStatus { get; set; }

        // Əsas məlumat
        public decimal Price { get; set; }
        public int RoomCount { get; set; }
        public double Area { get; set; }

        public int Floor { get; set; }
        public int TotalFloors { get; set; }

        public bool HasDocument { get; set; }   // Çıxarış
        public bool HasMortgage { get; set; }   // İpoteka

        // Əlaqə
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        // Location
        public Guid CityId { get; set; }
        public City City { get; set; }

        public Guid? DistrictId { get; set; }
        public District District { get; set; }

        public Guid? MetroId { get; set; }
        public Metro Metro { get; set; }

        // Multilanguage
        public ICollection<ListingTranslation> Translations { get; set; }

        // Images
        public ICollection<ListingImage> Images { get; set; }

        public Guid? AgencyId { get; set; }
        public Agency Agency { get; set; }

        public Guid? ResidentialComplexId { get; set; }
        public ResidentialComplex ResidentialComplex { get; set; }

        public CommercialDetail CommercialDetail { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
