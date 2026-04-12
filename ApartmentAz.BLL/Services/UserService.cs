using ApartmentAz.BLL.DTOs.User;
using ApartmentAz.BLL.Interfaces;
using ApartmentAz.BLL.Models;
using ApartmentAz.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace ApartmentAz.BLL.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;

    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<UserDto>> GetByIdAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
            return Result<UserDto>.Failure("User not found.");

        return Result<UserDto>.Success(MapToDto(user));
    }

    public async Task<Result<UserDto>> GetByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            return Result<UserDto>.Failure("User not found.");

        return Result<UserDto>.Success(MapToDto(user));
    }

    private static UserDto MapToDto(AppUser user) => new()
    {
        Id = user.Id,
        FullName = user.FullName,
        Email = user.Email!,
        PhoneNumber = user.PhoneNumber
    };
}
