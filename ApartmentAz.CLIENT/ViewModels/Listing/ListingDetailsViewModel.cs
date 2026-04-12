namespace ApartmentAz.CLIENT.ViewModels.Listing;

public class ListingDetailsViewModel
{
    public Guid Id { get; set; }
    public int ListingType { get; set; }
    public int? RentType { get; set; }
    public int PropertyType { get; set; }
    public int SellerType { get; set; }
    public int RepairStatus { get; set; }
    public decimal Price { get; set; }
    public int RoomCount { get; set; }
    public double Area { get; set; }
    public int Floor { get; set; }
    public int TotalFloors { get; set; }
    public bool HasDocument { get; set; }
    public bool HasMortgage { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? CityName { get; set; }
    public string? DistrictName { get; set; }
    public string? MetroName { get; set; }
    public Guid UserId { get; set; }
    public string? UserFullName { get; set; }
    public List<string> ImageUrls { get; set; } = [];
    public string? AgencyName { get; set; }
    public string? ResidentialComplexName { get; set; }
    public CommercialDetailViewModel? CommercialDetail { get; set; }
    public bool IsFavorited { get; set; }
    public bool IsOwner { get; set; }
}

public class CommercialDetailViewModel
{
    public bool HasParking { get; set; }
    public bool HasHeating { get; set; }
    public bool HasAirConditioner { get; set; }
}
