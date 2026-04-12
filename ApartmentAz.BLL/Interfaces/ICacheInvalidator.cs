namespace ApartmentAz.BLL.Interfaces;

public interface ICacheInvalidator
{
    void InvalidateListings();
    void InvalidateLocations();
}
