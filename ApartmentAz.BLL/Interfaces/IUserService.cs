using ApartmentAz.BLL.DTOs.User;
using ApartmentAz.BLL.Models;

namespace ApartmentAz.BLL.Interfaces;

public interface IUserService
{
    Task<Result<UserDto>> GetByIdAsync(Guid userId);
    Task<Result<UserDto>> GetByEmailAsync(string email);
}
