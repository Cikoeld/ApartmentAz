using ApartmentAz.BLL.DTOs;
using ApartmentAz.BLL.DTOs.Dashboard;
using ApartmentAz.BLL.DTOs.Listing;
using ApartmentAz.BLL.DTOs.User;
using ApartmentAz.BLL.Models;

namespace ApartmentAz.BLL.Interfaces;

public interface IAdminService
{
    // ── Dashboard ────────────────────────────────────────────────────────
    Task<Result<AdminDashboardStatsDto>> GetDashboardStatsAsync();
    Task<Result<AdminDashboardFullDto>> GetFullDashboardAsync(string lang);

    // ── Listings ─────────────────────────────────────────────────────────
    Task<Result<List<AdminListingDto>>> GetAllListingsAsync(string lang);
    Task<Result<bool>> ApproveListingAsync(Guid listingId);
    Task<Result<bool>> RejectListingAsync(Guid listingId);
    Task<Result<bool>> UpdateListingAsync(Guid listingId, UpdateListingDto dto);
    Task<Result<bool>> DeleteListingAsync(Guid listingId);

    // ── Users ────────────────────────────────────────────────────────────
    Task<Result<List<UserDto>>> GetAllUsersAsync();
    Task<Result<bool>> UpdateUserAsync(Guid userId, UpdateUserDto dto);
    Task<Result<bool>> BanUserAsync(Guid userId);
    Task<Result<bool>> UnbanUserAsync(Guid userId);
    Task<Result<bool>> DeleteUserAsync(Guid userId);
}
