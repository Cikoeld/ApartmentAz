using ApartmentAz.BLL.DTOs.Auth;
using ApartmentAz.BLL.Models;

namespace ApartmentAz.BLL.Interfaces;

public interface IAuthService
{
    Task<Result<AuthResultDto>> RegisterAsync(RegisterDto dto);
    Task<Result<AuthResultDto>> LoginAsync(LoginDto dto);
    Task LogoutAsync();
}
