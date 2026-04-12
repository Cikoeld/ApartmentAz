using ApartmentAz.CLIENT.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentAz.CLIENT.Controllers;

[Authorize]
public class FavoriteController : Controller
{
    private readonly ApiFavoriteService _favoriteService;

    public FavoriteController(ApiFavoriteService favoriteService)
    {
        _favoriteService = favoriteService;
    }

    public async Task<IActionResult> Index(string lang = "az")
    {
        var token = User.FindFirst("Token")?.Value ?? string.Empty;
        var favorites = await _favoriteService.GetAllAsync(token, lang);
        return View(favorites);
    }

    [HttpPost]
    public async Task<IActionResult> Toggle(Guid listingId, bool isFavorite)
    {
        var token = User.FindFirst("Token")?.Value ?? string.Empty;

        if (isFavorite)
            await _favoriteService.RemoveAsync(listingId, token);
        else
            await _favoriteService.AddAsync(listingId, token);

        return Ok();
    }
}
