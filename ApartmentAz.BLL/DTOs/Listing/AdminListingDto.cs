namespace ApartmentAz.BLL.DTOs.Listing;

public class AdminListingDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int RoomCount { get; set; }
    public double Area { get; set; }
    public int Floor { get; set; }
    public int TotalFloors { get; set; }
    public bool IsApproved { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CityName { get; set; }
    public string OwnerEmail { get; set; } = string.Empty;
    public string OwnerName  { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
}
