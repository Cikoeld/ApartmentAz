using Microsoft.AspNetCore.Identity;

namespace ApartmentAz.DAL.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FullName { get; set; } = null!;

        public ICollection<Listing> Listings { get; set; } = [];
    }
}