using ApartmentAz.BLL.DTOs.Favorite;
using ApartmentAz.BLL.Interfaces;
using ApartmentAz.BLL.Models;
using ApartmentAz.DAL.Interfaces;
using ApartmentAz.DAL.Models;

namespace ApartmentAz.BLL.Services;

public class FavoriteService : IFavoriteService
{
    private readonly IFavoriteRepository _favoriteRepository;

    public FavoriteService(IFavoriteRepository favoriteRepository)
    {
        _favoriteRepository = favoriteRepository;
    }

    public async Task<Result<bool>> AddAsync(Guid userId, Guid listingId)
    {
        var exists = await _favoriteRepository.ExistsAsync(userId, listingId);
        if (exists)
            return Result<bool>.Failure("Listing is already in favorites.");

        var favorite = new Favorite
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ListingId = listingId
        };

        await _favoriteRepository.AddAsync(favorite);

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> RemoveAsync(Guid userId, Guid listingId)
    {
        var exists = await _favoriteRepository.ExistsAsync(userId, listingId);
        if (!exists)
            return Result<bool>.Failure("Listing is not in favorites.");

        await _favoriteRepository.RemoveAsync(userId, listingId);

        return Result<bool>.Success(true);
    }

    public async Task<Result<List<FavoriteDto>>> GetUserFavoritesAsync(Guid userId, string lang)
    {
        var favorites = await _favoriteRepository.GetUserFavoritesAsync(userId);

        var result = favorites.Select(f =>
        {
            var translation = f.Listing?.Translations?.FirstOrDefault(t => t.LanguageCode == lang)
                              ?? f.Listing?.Translations?.FirstOrDefault(t => t.LanguageCode == "az");

            var cityTranslation = f.Listing?.City?.Translations?.FirstOrDefault(t => t.LanguageCode == lang)
                                  ?? f.Listing?.City?.Translations?.FirstOrDefault(t => t.LanguageCode == "az");

            return new FavoriteDto
            {
                ListingId  = f.ListingId,
                Title      = translation?.Title ?? string.Empty,
                Price      = f.Listing?.Price ?? 0,
                RoomCount  = f.Listing?.RoomCount ?? 0,
                Area       = f.Listing?.Area ?? 0,
                CityName   = cityTranslation?.Name,
                ThumbnailUrl = f.Listing?.Images?.FirstOrDefault()?.ImageUrl
            };
        }).ToList();

        return Result<List<FavoriteDto>>.Success(result);
    }

    public async Task<Result<bool>> ExistsAsync(Guid userId, Guid listingId)
    {
        var exists = await _favoriteRepository.ExistsAsync(userId, listingId);
        return Result<bool>.Success(exists);
    }
}
