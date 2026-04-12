namespace ApartmentAz.BLL.DTOs.Listing;

public class UpdateListingDto
{
    public decimal Price { get; set; }
    public int RoomCount { get; set; }
    public double Area { get; set; }
    public int Floor { get; set; }
    public int TotalFloors { get; set; }
}
