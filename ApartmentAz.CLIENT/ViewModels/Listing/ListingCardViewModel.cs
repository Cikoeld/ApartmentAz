namespace ApartmentAz.CLIENT.ViewModels.Listing;

public class ListingCardViewModel
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public int RoomCount { get; set; }
    public double Area { get; set; }
    public int Floor { get; set; }
    public int TotalFloors { get; set; }
    public int ListingType { get; set; }
    public int PropertyType { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? CityName { get; set; }
    public string? DistrictName { get; set; }
    public string? ThumbnailUrl { get; set; }
}
