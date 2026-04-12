namespace ApartmentAz.CLIENT.ViewModels.Favorite;

public class FavoriteViewModel
{
    public Guid ListingId { get; set; }
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public string? ThumbnailUrl { get; set; }
    public int RoomCount { get; set; }
    public double Area { get; set; }
    public string? CityName { get; set; }
}
