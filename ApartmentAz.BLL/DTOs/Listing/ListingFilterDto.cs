using ApartmentAz.DAL.Enums;

namespace ApartmentAz.BLL.DTOs.Listing;

public class ListingFilterDto
{
    public Guid? CityId { get; set; }
    public Guid? DistrictId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? RoomCount { get; set; }
    public string Lang { get; set; } = "az";

    // New filters
    public ListingType? ListingType { get; set; }
    public PropertyType? PropertyType { get; set; }
    public double? MinArea { get; set; }
    public double? MaxArea { get; set; }
    public RepairStatus? RepairStatus { get; set; }

    // Sorting
    public string? SortBy { get; set; }
}
