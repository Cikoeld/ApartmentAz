using ApartmentAz.BLL.DTOs.Auth;
using ApartmentAz.BLL.Models;

namespace ApartmentAz.BLL.Interfaces;

public interface IAuthService
{
    Task<Result<AuthResultDto>> RegisterAsync(RegisterDto dto);
    Task<Result<AuthResultDto>> LoginAsync(LoginDto dto);
    Task<Result<bool>> ForgotPasswordAsync(ForgotPasswordDto dto);
    Task<Result<bool>> ResetPasswordAsync(ResetPasswordDto dto);
    Task<Result<AuthResultDto>> RefreshTokenAsync(string refreshToken);
    Task<Result<bool>> RevokeTokenAsync(string refreshToken);
    Task<Result<bool>> RevokeAllTokensAsync(Guid userId);
    Task LogoutAsync();
}
