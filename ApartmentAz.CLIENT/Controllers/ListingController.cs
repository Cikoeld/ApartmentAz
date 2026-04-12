using ApartmentAz.CLIENT.Services;
using ApartmentAz.CLIENT.ViewModels.Listing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentAz.CLIENT.Controllers;

public class ListingController : Controller
{
    private readonly ApiListingService _listingService;
    private readonly ApiLocationService _locationService;
    private readonly ApiAgencyService _agencyService;
    private readonly ApiResidentialComplexService _rcService;
    private readonly ApiFavoriteService _favoriteService;
    private readonly ClientTranslationService _translationService;

    public ListingController(
        ApiListingService listingService,
        ApiLocationService locationService,
        ApiAgencyService agencyService,
        ApiResidentialComplexService rcService,
        ApiFavoriteService favoriteService,
        ClientTranslationService translationService)
    {
        _listingService = listingService;
        _locationService = locationService;
        _agencyService = agencyService;
        _rcService = rcService;
        _favoriteService = favoriteService;
        _translationService = translationService;
    }

    public async Task<IActionResult> Details(Guid id, string? lang = null)
    {
        var resolvedLang = ResolveLang(lang);
        var listing = await _listingService.GetByIdAsync(id, resolvedLang);
        if (listing == null)
            return NotFound();

        if (User.Identity?.IsAuthenticated == true)
        {
            var token = User.FindFirst("Token")?.Value ?? string.Empty;
            var favorites = await _favoriteService.GetAllAsync(token, resolvedLang);
            listing.IsFavorited = favorites.Any(f => f.ListingId == id);
            listing.IsOwner = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                              == listing.UserId.ToString();
        }

        return View(listing);
    }

    public async Task<IActionResult> Create(string? lang = null)
    {
        var resolvedLang = ResolveLang(lang);
        var vm = new CreateListingViewModel
        {
            Cities = await _locationService.GetCitiesAsync(resolvedLang),
            Agencies = await _agencyService.GetAllAsync(),
            ResidentialComplexes = await _rcService.GetAllAsync(resolvedLang)
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateListingViewModel vm, string? lang = null)
    {
        var resolvedLang = ResolveLang(lang);

        if (!ModelState.IsValid)
        {
            vm.Cities = await _locationService.GetCitiesAsync(resolvedLang);
            vm.Agencies = await _agencyService.GetAllAsync();
            vm.ResidentialComplexes = await _rcService.GetAllAsync(resolvedLang);
            return View(vm);
        }

        var token = User.FindFirst("Token")?.Value;

        // Auto-translate title and description to all 3 languages
        var titleTranslations = await _translationService.TranslateToAllAsync(vm.Title, resolvedLang);
        var descTranslations = await _translationService.TranslateToAllAsync(vm.Description, resolvedLang);

        vm.TitleAz = titleTranslations.GetValueOrDefault("az", vm.Title);
        vm.TitleEn = titleTranslations.GetValueOrDefault("en", vm.Title);
        vm.TitleRu = titleTranslations.GetValueOrDefault("ru", vm.Title);
        vm.DescriptionAz = descTranslations.GetValueOrDefault("az", vm.Description);
        vm.DescriptionEn = descTranslations.GetValueOrDefault("en", vm.Description);
        vm.DescriptionRu = descTranslations.GetValueOrDefault("ru", vm.Description);

        var response = string.IsNullOrEmpty(token)
            ? await _listingService.CreateRequestAsync(vm)
            : await _listingService.CreateAsync(vm, token);

        if (response.IsSuccessStatusCode)
            return RedirectToAction("Index", "Home", new { lang = resolvedLang });

        var body = await response.Content.ReadAsStringAsync();
        ModelState.AddModelError(string.Empty, string.IsNullOrWhiteSpace(body)
            ? "Failed to create listing."
            : $"Failed to create listing: {body}");

        vm.Cities = await _locationService.GetCitiesAsync(resolvedLang);
        vm.Agencies = await _agencyService.GetAllAsync();
        vm.ResidentialComplexes = await _rcService.GetAllAsync(resolvedLang);
        return View(vm);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var token = User.FindFirst("Token")?.Value;
        if (string.IsNullOrEmpty(token))
            return RedirectToAction("Login", "Auth");

        await _listingService.DeleteAsync(id, token);
        return RedirectToAction("Index", "Home", new { lang = ResolveLang(null) });
    }

    private string ResolveLang(string? urlParam)
    {
        if (!string.IsNullOrWhiteSpace(urlParam))
            return urlParam;

        if (Request.Query.TryGetValue("lang", out var q) && !string.IsNullOrWhiteSpace(q))
            return q.ToString();

        if (Request.Cookies.TryGetValue("lang", out var c) && !string.IsNullOrWhiteSpace(c))
            return c;

        return "az";
    }
}
