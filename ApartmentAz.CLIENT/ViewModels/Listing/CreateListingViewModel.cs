using ApartmentAz.CLIENT.ViewModels.Location;
using System.ComponentModel.DataAnnotations;

namespace ApartmentAz.CLIENT.ViewModels.Listing;

public class CreateListingViewModel
{
    [Required] public int ListingType { get; set; }
    public int? RentType { get; set; }
    [Required] public int PropertyType { get; set; }
    [Required] public int SellerType { get; set; }
    [Required] public int RepairStatus { get; set; }
    [Required, Range(1, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }
    [Required, Range(0, 20)] public int RoomCount { get; set; }
    [Required, Range(1, 10000)]  public double Area { get; set; }
    [Required] public int Floor { get; set; }
    [Required, Range(1, 200)]  public int TotalFloors { get; set; }
    public bool HasDocument { get; set; }
    public bool HasMortgage { get; set; }

    [Required] public string Name  { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] public string Phone { get; set; } = string.Empty;

    [Required] public Guid CityId { get; set; }
    public Guid? DistrictId { get; set; }
    public Guid? MetroId { get; set; }
    public Guid? AgencyId { get; set; }
    public Guid? ResidentialComplexId { get; set; }

    // Translations
    [Required] public string Title       { get; set; } = string.Empty;
    [Required] public string Description { get; set; } = string.Empty;

    // Filled by auto-translation — not submitted by the user
    public string TitleAz       { get; set; } = string.Empty;
    public string TitleEn       { get; set; } = string.Empty;
    public string TitleRu       { get; set; } = string.Empty;
    public string DescriptionAz { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public string DescriptionRu { get; set; } = string.Empty;

    // Images
    public List<IFormFile> Images { get; set; } = [];

    // Commercial
    public bool HasParking { get; set; }
    public bool HasHeating { get; set; }
    public bool HasAirConditioner { get; set; }

    // Dropdowns (not submitted, just for rendering)
    public List<SelectItemViewModel> Cities { get; set; } = [];
    public List<SelectItemViewModel> Agencies { get; set; } = [];
    public List<SelectItemViewModel> ResidentialComplexes { get; set; } = [];
}
