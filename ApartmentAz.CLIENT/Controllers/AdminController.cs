using ApartmentAz.CLIENT.Services;
using ApartmentAz.CLIENT.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentAz.CLIENT.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApiAdminService _adminService;

    public AdminController(ApiAdminService adminService)
    {
        _adminService = adminService;
    }

    public async Task<IActionResult> Dashboard()
    {
        var lang = Request.Cookies["lang"] ?? "az";
        var data = await _adminService.GetFullDashboardAsync(GetToken(), lang);

        if (data == null)
        {
            return View(new DashboardViewModel { LoadFailed = true });
        }

        var vm = new DashboardViewModel
        {
            TotalListings    = data.TotalListings,
            PendingListings  = data.PendingListings,
            ApprovedListings = data.ApprovedListings,
            TotalUsers       = data.TotalUsers,
            BannedUsers      = data.BannedUsers,
            ListingsPerDayLabels        = data.ChartData.ListingsPerDayLabels,
            ListingsPerDayValues        = data.ChartData.ListingsPerDayValues,
            RegistrationsPerMonthLabels = data.ChartData.RegistrationsPerMonthLabels,
            RegistrationsPerMonthValues = data.ChartData.RegistrationsPerMonthValues,
            RecentPendingListings = data.RecentPendingListings,
            RecentUsers           = data.RecentUsers
        };

        return View(vm);
    }

    public async Task<IActionResult> Listings()
    {
        var token = GetToken();
        var lang = Request.Cookies["lang"] ?? "az";
        var listings = await _adminService.GetListingsAsync(token, lang);
        return View(listings);
    }

    [HttpPost]
    public async Task<IActionResult> ApproveListing(Guid id)
    {
        var success = await _adminService.ApproveListingAsync(id, GetToken());
        return Json(new { success });
    }

    [HttpPost]
    public async Task<IActionResult> RejectListing(Guid id)
    {
        var success = await _adminService.RejectListingAsync(id, GetToken());
        return Json(new { success });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateListing(Guid id, [FromBody] UpdateListingModel model)
    {
        var success = await _adminService.UpdateListingAsync(id, model, GetToken());
        return Json(new { success });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteListing(Guid id)
    {
        var success = await _adminService.DeleteListingAsync(id, GetToken());
        return Json(new { success });
    }

    public async Task<IActionResult> Users()
    {
        var users = await _adminService.GetUsersAsync(GetToken());
        return View(users);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserModel model)
    {
        var success = await _adminService.UpdateUserAsync(id, model, GetToken());
        return Json(new { success });
    }

    [HttpPost]
    public async Task<IActionResult> BanUser(Guid id)
    {
        var success = await _adminService.BanUserAsync(id, GetToken());
        return Json(new { success });
    }

    [HttpPost]
    public async Task<IActionResult> UnbanUser(Guid id)
    {
        var success = await _adminService.UnbanUserAsync(id, GetToken());
        return Json(new { success });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var success = await _adminService.DeleteUserAsync(id, GetToken());
        return Json(new { success });
    }

    private string GetToken()
        => User.FindFirst("Token")?.Value ?? string.Empty;
}

public class UpdateListingModel
{
    public decimal Price { get; set; }
    public int RoomCount { get; set; }
    public double Area { get; set; }
    public int Floor { get; set; }
    public int TotalFloors { get; set; }
}

public class UpdateUserModel
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
}
