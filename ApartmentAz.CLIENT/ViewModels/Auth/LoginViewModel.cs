using System.ComponentModel.DataAnnotations;

namespace ApartmentAz.CLIENT.ViewModels.Auth;

public class LoginViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = null!;

    [Required, MinLength(6)]
    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; }
}
