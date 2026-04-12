using ApartmentAz.CLIENT.ViewModels.Location;

namespace ApartmentAz.CLIENT.ViewModels.Listing;

public class ListingFilterViewModel
{
    public Guid? CityId { get; set; }
    public Guid? DistrictId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? RoomCount { get; set; }
    public string? Lang { get; set; }

    // New filters
    public int? ListingType { get; set; }
    public int? PropertyType { get; set; }
    public double? MinArea { get; set; }
    public double? MaxArea { get; set; }
    public int? RepairStatus { get; set; }

    // Sorting
    public string? SortBy { get; set; }

    public List<ListingCardViewModel> Listings { get; set; } = [];
    public List<SelectItemViewModel> Cities { get; set; } = [];
    public HashSet<Guid> FavoriteListingIds { get; set; } = [];
}
