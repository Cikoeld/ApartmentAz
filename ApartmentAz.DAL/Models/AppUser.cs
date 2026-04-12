using Microsoft.AspNetCore.Identity;

namespace ApartmentAz.DAL.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FullName { get; set; } = null!;
        public bool IsBanned { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Listing> Listings { get; set; } = [];
        public ICollection<Message> SentMessages { get; set; } = [];
        public ICollection<Message> ReceivedMessages { get; set; } = [];
    }
}