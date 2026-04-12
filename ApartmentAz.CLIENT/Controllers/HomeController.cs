using ApartmentAz.CLIENT.Models;
using ApartmentAz.CLIENT.Services;
using ApartmentAz.CLIENT.ViewModels.Listing;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ApartmentAz.CLIENT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiListingService _listingService;
        private readonly ApiLocationService _locationService;
        private readonly ApiFavoriteService _favoriteService;

        public HomeController(
            ApiListingService listingService,
            ApiLocationService locationService,
            ApiFavoriteService favoriteService)
        {
            _listingService = listingService;
            _locationService = locationService;
            _favoriteService = favoriteService;
        }

        public async Task<IActionResult> Index(ListingFilterViewModel filter)
        {
            var lang = ResolveLanguage();
            filter.Lang = lang;
            if (filter.PageNumber < 1) filter.PageNumber = 1;
            if (filter.PageSize < 1)   filter.PageSize   = 12;

            var pagedTask  = _listingService.GetAllAsync(filter, lang);
            var citiesTask = _locationService.GetCitiesAsync(lang);

            await Task.WhenAll(pagedTask, citiesTask);

            var paged = pagedTask.Result;
            filter.Listings    = paged.Items;
            filter.TotalCount  = paged.TotalCount;
            filter.TotalPages  = paged.TotalPages;
            filter.Cities      = citiesTask.Result;

            if (User.Identity?.IsAuthenticated == true)
            {
                var token = User.FindFirst("Token")?.Value ?? string.Empty;
                var favorites = await _favoriteService.GetAllAsync(token, lang);
                filter.FavoriteListingIds = [.. favorites.Select(f => f.ListingId)];
            }

            return View(filter);
        }

        public async Task<IActionResult> GetDistricts(Guid cityId, string? lang = null)
        {
            var resolvedLang = !string.IsNullOrEmpty(lang) ? lang : (Request.Cookies["lang"] ?? "az");
            var districts = await _locationService.GetDistrictsAsync(cityId, resolvedLang);
            return Json(districts);
        }

        public async Task<IActionResult> GetMetros(Guid cityId, string? lang = null)
        {
            var resolvedLang = !string.IsNullOrEmpty(lang) ? lang : (Request.Cookies["lang"] ?? "az");
            var metros = await _locationService.GetMetrosAsync(cityId, resolvedLang);
            return Json(metros);
        }

        private string ResolveLanguage()
        {
            // Request.Query is case-insensitive — handles both ?lang=ru and ?Lang=ru (form hidden field)
            if (Request.Query.TryGetValue("lang", out var qLang) && !string.IsNullOrWhiteSpace(qLang))
                return qLang.ToString();

            if (Request.Cookies.TryGetValue("lang", out var cLang) && !string.IsNullOrWhiteSpace(cLang))
                return cLang;

            return "az";
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
