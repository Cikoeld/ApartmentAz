using ApartmentAz.DAL.Enums;

namespace ApartmentAz.BLL.DTOs.Listing;

public class ListingDto
{
    public Guid Id { get; set; }

    public decimal Price { get; set; }
    public int RoomCount { get; set; }
    public double Area { get; set; }
    public int Floor { get; set; }
    public int TotalFloors { get; set; }

    public ListingType ListingType { get; set; }
    public PropertyType PropertyType { get; set; }

    // Translated fields
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;

    // Location
    public string? CityName { get; set; }
    public string? DistrictName { get; set; }

    // First image as thumbnail
    public string? ThumbnailUrl { get; set; }
}
