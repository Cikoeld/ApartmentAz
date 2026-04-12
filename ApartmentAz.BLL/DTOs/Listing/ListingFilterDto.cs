using ApartmentAz.DAL.Enums;

namespace ApartmentAz.BLL.DTOs.Listing;

public class ListingFilterDto
{
    public Guid? CityId { get; set; }
    public Guid? DistrictId { get; set; }
    public Guid? MetroId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? RoomCount { get; set; }
    public string Lang { get; set; } = "az";

    // Enum filters
    public ListingType? ListingType { get; set; }
    public PropertyType? PropertyType { get; set; }
    public RepairStatus? RepairStatus { get; set; }

    // Area range
    public double? MinArea { get; set; }
    public double? MaxArea { get; set; }

    // Sorting
    public string? SortBy { get; set; }

    // Search
    public string? SearchQuery { get; set; }

    // Pagination
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}
