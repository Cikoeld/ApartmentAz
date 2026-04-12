namespace ApartmentAz.BLL.DTOs.Dashboard;

public class RecentPendingListingDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? CityName { get; set; }
    public DateTime CreatedAt { get; set; }
}
