using ApartmentAz.BLL.DTOs.Listing;
using ApartmentAz.BLL.Interfaces;
using ApartmentAz.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApartmentAz.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListingsController : ControllerBase
{
    private readonly IListingService _listingService;
    private readonly UserManager<AppUser> _userManager;

    public ListingsController(IListingService listingService, UserManager<AppUser> userManager)
    {
        _listingService = listingService;
        _userManager = userManager;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] CreateListingDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        var result = await _listingService.CreateAsync(dto, userId);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.ErrorMessage });

        return CreatedAtAction(nameof(GetById), new { id = result.Data }, new { id = result.Data });
    }

    [HttpPost("request")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateRequest([FromForm] CreateListingDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var guestOwnerId = await GetOrCreateGuestOwnerIdAsync();
        var result = await _listingService.CreateAsync(dto, guestOwnerId);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.ErrorMessage });

        return Ok(new
        {
            message = "Your listing request was submitted and is pending admin approval.",
            id = result.Data
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ListingFilterDto filter)
    {
        var result = await _listingService.GetAllAsync(filter);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.ErrorMessage });

        return Ok(result.Data);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] ListingFilterDto filter)
    {
        filter.SearchQuery = q;
        var result = await _listingService.GetAllAsync(filter);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.ErrorMessage });

        return Ok(result.Data);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, [FromQuery] string lang = "az")
    {
        var result = await _listingService.GetByIdAsync(id, lang);

        if (!result.IsSuccess)
            return NotFound(new { error = result.ErrorMessage });

        return Ok(result.Data);
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = GetUserId();
        var result = await _listingService.DeleteAsync(id, userId);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.ErrorMessage });

        return NoContent();
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(claim!);
    }

    private async Task<Guid> GetOrCreateGuestOwnerIdAsync()
    {
        const string guestEmail = "guest-listing@apartmentaz.com";

        var guest = await _userManager.FindByEmailAsync(guestEmail);
        if (guest != null)
            return guest.Id;

        guest = new AppUser
        {
            FullName = "Guest Listing Owner",
            Email = guestEmail,
            UserName = guestEmail,
            EmailConfirmed = true
        };

        var createResult = await _userManager.CreateAsync(guest, "Guest123");
        if (!createResult.Succeeded)
        {
            var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to create guest listing owner account: {errors}");
        }

        await _userManager.AddToRoleAsync(guest, "User");
        return guest.Id;
    }
}
