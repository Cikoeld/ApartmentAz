using ApartmentAz.DAL.Enums;

namespace ApartmentAz.BLL.DTOs.Listing;

public class ListingDetailsDto
{
    public Guid Id { get; set; }

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

    // Translated fields
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;

    // Location
    public string? CityName { get; set; }
    public string? DistrictName { get; set; }
    public string? MetroName { get; set; }

    // Owner
    public Guid UserId { get; set; }
    public string? UserFullName { get; set; }

    // Images
    public List<string> ImageUrls { get; set; } = [];

    // Agency
    public string? AgencyName { get; set; }

    // Residential Complex
    public string? ResidentialComplexName { get; set; }

    // Commercial Detail
    public ListingCommercialDetailDto? CommercialDetail { get; set; }
}

public class ListingCommercialDetailDto
{
    public bool HasParking { get; set; }
    public bool HasHeating { get; set; }
    public bool HasAirConditioner { get; set; }
}
