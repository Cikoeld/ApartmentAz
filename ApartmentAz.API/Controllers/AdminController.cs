using ApartmentAz.BLL.DTOs.Listing;
using ApartmentAz.BLL.DTOs.User;
using ApartmentAz.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentAz.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    // ── Dashboard ─────────────────────────────────────────────────────────

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var result = await _adminService.GetDashboardStatsAsync();
        return result.IsSuccess ? Ok(result.Data) : BadRequest(new { error = result.ErrorMessage });
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetFullDashboard([FromQuery] string lang = "az")
    {
        var result = await _adminService.GetFullDashboardAsync(lang);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(new { error = result.ErrorMessage });
    }

    // ── Listings ─────────────────────────────────────────────────────────

    [HttpGet("listings")]
    public async Task<IActionResult> GetAllListings([FromQuery] string lang = "az")
    {
        var result = await _adminService.GetAllListingsAsync(lang);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(new { error = result.ErrorMessage });
    }

    [HttpPut("listings/{id:guid}/approve")]
    public async Task<IActionResult> ApproveListing(Guid id)
    {
        var result = await _adminService.ApproveListingAsync(id);
        return result.IsSuccess ? Ok() : BadRequest(new { error = result.ErrorMessage });
    }

    [HttpPut("listings/{id:guid}/reject")]
    public async Task<IActionResult> RejectListing(Guid id)
    {
        var result = await _adminService.RejectListingAsync(id);
        return result.IsSuccess ? Ok() : BadRequest(new { error = result.ErrorMessage });
    }

    [HttpPut("listings/{id:guid}")]
    public async Task<IActionResult> UpdateListing(Guid id, [FromBody] UpdateListingDto dto)
    {
        var result = await _adminService.UpdateListingAsync(id, dto);
        return result.IsSuccess ? Ok() : BadRequest(new { error = result.ErrorMessage });
    }

    [HttpDelete("listings/{id:guid}")]
    public async Task<IActionResult> DeleteListing(Guid id)
    {
        var result = await _adminService.DeleteListingAsync(id);
        return result.IsSuccess ? NoContent() : BadRequest(new { error = result.ErrorMessage });
    }

    // ── Users ────────────────────────────────────────────────────────────

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _adminService.GetAllUsersAsync();
        return result.IsSuccess ? Ok(result.Data) : BadRequest(new { error = result.ErrorMessage });
    }

    [HttpPut("users/{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto dto)
    {
        var result = await _adminService.UpdateUserAsync(id, dto);
        return result.IsSuccess ? Ok() : BadRequest(new { error = result.ErrorMessage });
    }

    [HttpPut("users/{id:guid}/ban")]
    public async Task<IActionResult> BanUser(Guid id)
    {
        var result = await _adminService.BanUserAsync(id);
        return result.IsSuccess ? Ok() : BadRequest(new { error = result.ErrorMessage });
    }

    [HttpPut("users/{id:guid}/unban")]
    public async Task<IActionResult> UnbanUser(Guid id)
    {
        var result = await _adminService.UnbanUserAsync(id);
        return result.IsSuccess ? Ok() : BadRequest(new { error = result.ErrorMessage });
    }

    [HttpDelete("users/{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await _adminService.DeleteUserAsync(id);
        return result.IsSuccess ? NoContent() : BadRequest(new { error = result.ErrorMessage });
    }
}
