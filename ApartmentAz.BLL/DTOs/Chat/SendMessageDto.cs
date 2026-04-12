namespace ApartmentAz.BLL.DTOs.Chat;

public class SendMessageDto
{
    public Guid ReceiverId { get; set; }
    public Guid ListingId { get; set; }
    public string Content { get; set; } = string.Empty;
}
