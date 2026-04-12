namespace ApartmentAz.BLL.DTOs.Auth;

public class AuthResultDto
{
    public bool Succeeded { get; set; }
    public string? Token { get; set; }
    public string? ErrorMessage { get; set; }
    public Guid? UserId { get; set; }
}
