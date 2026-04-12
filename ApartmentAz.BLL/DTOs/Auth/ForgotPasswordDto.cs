using System.ComponentModel.DataAnnotations;

namespace ApartmentAz.BLL.DTOs.Auth;

public class ForgotPasswordDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}
