using System.ComponentModel.DataAnnotations;

namespace ApartmentAz.CLIENT.ViewModels.Auth;

public class RegisterViewModel
{
    [Required]
    public string FullName { get; set; } = null!;

    [Required, EmailAddress]
    public string Email { get; set; } = null!;

    [Required, MinLength(6)]
    public string Password { get; set; } = null!;

    [Compare("Password")]
    public string ConfirmPassword { get; set; } = null!;

    public string? PhoneNumber { get; set; }
}
