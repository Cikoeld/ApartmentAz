using ApartmentAz.DAL.Enums;
using Microsoft.AspNetCore.Http;

namespace ApartmentAz.BLL.DTOs.Listing;

public class CreateListingDto
{
    // Enums
    public ListingType ListingType { get; set; }
    public RentType? RentType { get; set; }
    public PropertyType PropertyType { get; set; }
    public SellerType SellerType { get; set; }
    public RepairStatus RepairStatus { get; set; }

    // Core info
    public decimal Price { get; set; }
    public int RoomCount { get; set; }
    public double Area { get; set; }
    public int Floor { get; set; }
    public int TotalFloors { get; set; }
    public bool HasDocument { get; set; }
    public bool HasMortgage { get; set; }

    // Contact
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;

    // Location
    public Guid CityId { get; set; }
    public Guid? DistrictId { get; set; }
    public Guid? MetroId { get; set; }

    // Optional relations
    public Guid? AgencyId { get; set; }
    public Guid? ResidentialComplexId { get; set; }

    // Translations (az, en, ru)
    public List<CreateListingTranslationDto> Translations { get; set; } = [];

    // Images
    public List<IFormFile> Images { get; set; } = [];

    // Commercial detail (only for PropertyType.Commercial)
    public CreateCommercialDetailDto? CommercialDetail { get; set; }
}

public class CreateListingTranslationDto
{
    public string LanguageCode { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
}

public class CreateCommercialDetailDto
{
    public bool HasParking { get; set; }
    public bool HasHeating { get; set; }
    public bool HasAirConditioner { get; set; }
}
