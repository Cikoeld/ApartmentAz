using ApartmentAz.BLL.DTOs;
using ApartmentAz.BLL.DTOs.Dashboard;
using ApartmentAz.BLL.DTOs.Listing;
using ApartmentAz.BLL.DTOs.User;
using ApartmentAz.BLL.Interfaces;
using ApartmentAz.BLL.Models;
using ApartmentAz.DAL.Interfaces;
using ApartmentAz.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApartmentAz.BLL.Services;

public class AdminService : IAdminService
{
    private readonly IListingRepository _listingRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IPhotoService _photoService;

    public AdminService(
        IListingRepository listingRepository,
        UserManager<AppUser> userManager,
        IPhotoService photoService)
    {
        _listingRepository = listingRepository;
        _userManager = userManager;
        _photoService = photoService;
    }

    // ── Dashboard ─────────────────────────────────────────────────────────

    public async Task<Result<AdminDashboardStatsDto>> GetDashboardStatsAsync()
    {
        var listingQuery = _listingRepository.GetQueryable();
        var totalListings   = await listingQuery.CountAsync();
        var pendingListings  = await listingQuery.CountAsync(x => !x.IsApproved);
        var approvedListings = await listingQuery.CountAsync(x => x.IsApproved);
        var totalUsers  = await _userManager.Users.CountAsync();
        var bannedUsers = await _userManager.Users.CountAsync(u => u.IsBanned);

        return Result<AdminDashboardStatsDto>.Success(new AdminDashboardStatsDto
        {
            TotalListings    = totalListings,
            PendingListings  = pendingListings,
            ApprovedListings = approvedListings,
            TotalUsers  = totalUsers,
            BannedUsers = bannedUsers
        });
    }

    public async Task<Result<AdminDashboardFullDto>> GetFullDashboardAsync(string lang)
    {
        var listingQuery = _listingRepository.GetQueryable();

        // ── Stats ────────────────────────────────────────────────────────
        var totalListings    = await listingQuery.CountAsync();
        var pendingListings  = await listingQuery.CountAsync(x => !x.IsApproved);
        var approvedListings = await listingQuery.CountAsync(x => x.IsApproved);
        var totalUsers  = await _userManager.Users.CountAsync();
        var bannedUsers = await _userManager.Users.CountAsync(u => u.IsBanned);

        // ── Chart: Listings per day (last 7 days) ────────────────────────
        var sevenDaysAgo = DateTime.UtcNow.Date.AddDays(-6);
        var listingsPerDay = await listingQuery
            .Where(l => l.CreatedAt >= sevenDaysAgo)
            .GroupBy(l => l.CreatedAt.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .OrderBy(x => x.Date)
            .ToListAsync();

        var lpLabels = new List<string>();
        var lpValues = new List<int>();
        for (int i = 0; i < 7; i++)
        {
            var date = sevenDaysAgo.AddDays(i);
            lpLabels.Add(date.ToString("dd MMM"));
            lpValues.Add(listingsPerDay.FirstOrDefault(x => x.Date == date)?.Count ?? 0);
        }

        // ── Chart: User registrations per month (last 6 months) ──────────
        var sixMonthsAgo = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).AddMonths(-5);
        var registrationsPerMonth = await _userManager.Users
            .Where(u => u.CreatedAt >= sixMonthsAgo)
            .GroupBy(u => new { u.CreatedAt.Year, u.CreatedAt.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, Count = g.Count() })
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .ToListAsync();

        var regLabels = new List<string>();
        var regValues = new List<int>();
        for (int i = 0; i < 6; i++)
        {
            var monthDate = sixMonthsAgo.AddMonths(i);
            regLabels.Add(monthDate.ToString("MMM yyyy"));
            regValues.Add(registrationsPerMonth
                .FirstOrDefault(x => x.Year == monthDate.Year && x.Month == monthDate.Month)?.Count ?? 0);
        }

        // ── Recent pending listings (top 5) ──────────────────────────────
        var recentPending = await listingQuery
            .Where(l => !l.IsApproved)
            .OrderByDescending(l => l.CreatedAt)
            .Take(5)
            .Select(l => new RecentPendingListingDto
            {
                Id = l.Id,
                Price = l.Price,
                CreatedAt = l.CreatedAt,
                Title = l.Translations
                    .Where(t => t.LanguageCode == lang)
                    .Select(t => t.Title)
                    .FirstOrDefault()
                    ?? l.Translations
                        .Where(t => t.LanguageCode == "az")
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                CityName = l.City.Translations
                    .Where(t => t.LanguageCode == lang)
                    .Select(t => t.Name)
                    .FirstOrDefault()
                    ?? l.City.Translations
                        .Where(t => t.LanguageCode == "az")
                        .Select(t => t.Name)
                        .FirstOrDefault()
            })
            .ToListAsync();

        // ── Recent users (top 5) ─────────────────────────────────────────
        var recentUsers = await _userManager.Users
            .OrderByDescending(u => u.CreatedAt)
            .Take(5)
            .Select(u => new RecentUserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email!,
                CreatedAt = u.CreatedAt
            })
            .ToListAsync();

        return Result<AdminDashboardFullDto>.Success(new AdminDashboardFullDto
        {
            TotalListings    = totalListings,
            PendingListings  = pendingListings,
            ApprovedListings = approvedListings,
            TotalUsers       = totalUsers,
            BannedUsers      = bannedUsers,
            ChartData = new DashboardChartDataDto
            {
                ListingsPerDayLabels       = lpLabels,
                ListingsPerDayValues       = lpValues,
                RegistrationsPerMonthLabels = regLabels,
                RegistrationsPerMonthValues = regValues
            },
            RecentPendingListings = recentPending,
            RecentUsers           = recentUsers
        });
    }

    // ── Listings ─────────────────────────────────────────────────────────

    public async Task<Result<List<AdminListingDto>>> GetAllListingsAsync(string lang)
    {
        var items = await _listingRepository.GetQueryable()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new AdminListingDto
            {
                Id = x.Id,
                Price = x.Price,
                RoomCount = x.RoomCount,
                Area = x.Area,
                Floor = x.Floor,
                TotalFloors = x.TotalFloors,
                IsApproved = x.IsApproved,
                CreatedAt = x.CreatedAt,
                OwnerEmail = x.User.Email!,
                OwnerName  = x.User.FullName,
                ThumbnailUrl = x.Images.Select(i => i.ImageUrl).FirstOrDefault(),
                Title = x.Translations
                    .Where(t => t.LanguageCode == lang)
                    .Select(t => t.Title)
                    .FirstOrDefault()
                    ?? x.Translations
                        .Where(t => t.LanguageCode == "az")
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,
                CityName = x.City.Translations
                    .Where(t => t.LanguageCode == lang)
                    .Select(t => t.Name)
                    .FirstOrDefault()
                    ?? x.City.Translations
                        .Where(t => t.LanguageCode == "az")
                        .Select(t => t.Name)
                        .FirstOrDefault()
            })
            .ToListAsync();

        return Result<List<AdminListingDto>>.Success(items);
    }

    public async Task<Result<bool>> ApproveListingAsync(Guid listingId)
    {
        var listing = await _listingRepository.GetByIdAsync(listingId);
        if (listing == null)
            return Result<bool>.Failure("Listing not found.");

        listing.IsApproved = true;
        await _listingRepository.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> RejectListingAsync(Guid listingId)
    {
        var listing = await _listingRepository.GetByIdAsync(listingId);
        if (listing == null)
            return Result<bool>.Failure("Listing not found.");

        listing.IsApproved = false;
        await _listingRepository.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> UpdateListingAsync(Guid listingId, UpdateListingDto dto)
    {
        var listing = await _listingRepository.GetByIdAsync(listingId);
        if (listing == null)
            return Result<bool>.Failure("Listing not found.");

        listing.Price       = dto.Price;
        listing.RoomCount   = dto.RoomCount;
        listing.Area        = dto.Area;
        listing.Floor       = dto.Floor;
        listing.TotalFloors = dto.TotalFloors;

        await _listingRepository.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeleteListingAsync(Guid listingId)
    {
        var listing = await _listingRepository.GetByIdAsync(listingId);
        if (listing == null)
            return Result<bool>.Failure("Listing not found.");

        if (listing.Images != null)
        {
            foreach (var image in listing.Images)
                _photoService.DeleteImage(image.ImageUrl);
        }

        await _listingRepository.DeleteAsync(listing);
        return Result<bool>.Success(true);
    }

    // ── Users ────────────────────────────────────────────────────────────

    public async Task<Result<List<UserDto>>> GetAllUsersAsync()
    {
        var users = await _userManager.Users
            .OrderBy(u => u.FullName)
            .ToListAsync();

        var result = new List<UserDto>();
        foreach (var user in users)
        {
            var roles = (await _userManager.GetRolesAsync(user)).ToList();
            result.Add(new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber,
                IsBanned = user.IsBanned,
                Roles = roles
            });
        }

        return Result<List<UserDto>>.Success(result);
    }

    public async Task<Result<bool>> UpdateUserAsync(Guid userId, UpdateUserDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return Result<bool>.Failure("User not found.");

        // Check if new email is taken by another user
        if (!string.Equals(user.Email, dto.Email, StringComparison.OrdinalIgnoreCase))
        {
            var existing = await _userManager.FindByEmailAsync(dto.Email);
            if (existing != null && existing.Id != userId)
                return Result<bool>.Failure("Email is already in use.");
            user.Email    = dto.Email;
            user.UserName = dto.Email;
            user.NormalizedEmail    = dto.Email.ToUpperInvariant();
            user.NormalizedUserName = dto.Email.ToUpperInvariant();
        }

        user.FullName    = dto.FullName;
        user.PhoneNumber = dto.PhoneNumber;

        await _userManager.UpdateAsync(user);
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> BanUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return Result<bool>.Failure("User not found.");

        user.IsBanned = true;
        await _userManager.UpdateAsync(user);
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> UnbanUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return Result<bool>.Failure("User not found.");

        user.IsBanned = false;
        await _userManager.UpdateAsync(user);
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeleteUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return Result<bool>.Failure("User not found.");

        await _userManager.DeleteAsync(user);
        return Result<bool>.Success(true);
    }
}
