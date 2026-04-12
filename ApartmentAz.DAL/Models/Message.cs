namespace ApartmentAz.DAL.Models;

public class Message
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public AppUser Sender { get; set; } = null!;
    public Guid ReceiverId { get; set; }
    public AppUser Receiver { get; set; } = null!;
    public Guid ListingId { get; set; }
    public Listing Listing { get; set; } = null!;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; }
}
