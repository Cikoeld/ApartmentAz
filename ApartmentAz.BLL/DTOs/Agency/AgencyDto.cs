namespace ApartmentAz.BLL.DTOs.Agency;

public class AgencyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public int ListingCount { get; set; }
}
