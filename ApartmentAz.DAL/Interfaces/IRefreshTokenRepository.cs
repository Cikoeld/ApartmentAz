using ApartmentAz.DAL.Models;

namespace ApartmentAz.DAL.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token);
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task RevokeAllForUserAsync(Guid userId);
    Task SaveChangesAsync();
}
