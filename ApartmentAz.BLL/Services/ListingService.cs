using ApartmentAz.BLL.DTOs.Listing;
using ApartmentAz.BLL.Interfaces;
using ApartmentAz.BLL.Models;
using ApartmentAz.DAL.Enums;
using ApartmentAz.DAL.Interfaces;
using ApartmentAz.DAL.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApartmentAz.BLL.Services;

public class ListingService : IListingService
{
    private readonly IListingRepository _listingRepository;
    private readonly IPhotoService _photoService;
    private readonly ITranslationService _translationService;
    private readonly IMapper _mapper;

    public ListingService(
        IListingRepository listingRepository,
        IPhotoService photoService,
        ITranslationService translationService,
        IMapper mapper)
    {
        _listingRepository = listingRepository;
        _photoService = photoService;
        _translationService = translationService;
        _mapper = mapper;
    }

    public async Task<Result<Guid>> CreateAsync(CreateListingDto dto, Guid userId)
    {
        if (dto.Translations == null || dto.Translations.Count == 0)
            return Result<Guid>.Failure("At least one translation is required.");

        // Auto-translate missing languages
        await AutoTranslateAsync(dto.Translations);

        if (dto.PropertyType == PropertyType.Commercial && dto.CommercialDetail == null)
            return Result<Guid>.Failure("CommercialDetail is required for commercial property type.");

        var listing = _mapper.Map<Listing>(dto);
        listing.Id = Guid.NewGuid();
        listing.UserId = userId;
        listing.CreatedAt = DateTime.UtcNow;
        listing.IsApproved = true;

        // Map translations
        listing.Translations = dto.Translations.Select(t =>
        {
            var translation = _mapper.Map<ListingTranslation>(t);
            translation.Id = Guid.NewGuid();
            translation.ListingId = listing.Id;
            return translation;
        }).ToList();

        // Save images to wwwroot
        if (dto.Images is { Count: > 0 })
        {
            var imageUrls = await _photoService.SaveImagesAsync(dto.Images, "listings");
            listing.Images = imageUrls.Select(url => new ListingImage
            {
                Id = Guid.NewGuid(),
                ImageUrl = url,
                ListingId = listing.Id
            }).ToList();
        }

        // CommercialDetail
        if (dto.PropertyType == PropertyType.Commercial && dto.CommercialDetail != null)
        {
            var commercialDetail = _mapper.Map<CommercialDetail>(dto.CommercialDetail);
            commercialDetail.Id = Guid.NewGuid();
            commercialDetail.ListingId = listing.Id;
            listing.CommercialDetail = commercialDetail;
        }

        await _listingRepository.CreateAsync(listing);

        return Result<Guid>.Success(listing.Id);
    }

    public async Task<Result<PagedResult<ListingDto>>> GetAllAsync(ListingFilterDto filter)
    {
        var lang = filter.Lang ?? "az";
        var pageNumber = filter.PageNumber < 1 ? 1 : filter.PageNumber;
        var pageSize = filter.PageSize is < 1 or > 50 ? 12 : filter.PageSize;

        // ── 1. Start with no-tracking IQueryable (no Includes needed) ──────
        var query = _listingRepository.GetQueryable()
            .Where(x => x.IsApproved); // Only show approved listings to public

        // ── 2. Dynamic filters — all executed in SQL ────────────────────────
        if (filter.CityId.HasValue)
            query = query.Where(x => x.CityId == filter.CityId.Value);

        if (filter.DistrictId.HasValue)
            query = query.Where(x => x.DistrictId == filter.DistrictId.Value);

        if (filter.MetroId.HasValue)
            query = query.Where(x => x.MetroId == filter.MetroId.Value);

        if (filter.MinPrice.HasValue)
            query = query.Where(x => x.Price >= filter.MinPrice.Value);

        if (filter.MaxPrice.HasValue)
            query = query.Where(x => x.Price <= filter.MaxPrice.Value);

        if (filter.RoomCount.HasValue)
            query = query.Where(x => x.RoomCount == filter.RoomCount.Value);

        if (filter.ListingType.HasValue)
            query = query.Where(x => x.ListingType == filter.ListingType.Value);

        if (filter.PropertyType.HasValue)
            query = query.Where(x => x.PropertyType == filter.PropertyType.Value);

        if (filter.RepairStatus.HasValue)
            query = query.Where(x => x.RepairStatus == filter.RepairStatus.Value);

        if (filter.MinArea.HasValue)
            query = query.Where(x => x.Area >= filter.MinArea.Value);

        if (filter.MaxArea.HasValue)
            query = query.Where(x => x.Area <= filter.MaxArea.Value);

        // ── 3. Count before pagination (single COUNT query in SQL) ──────────
        var totalCount = await query.CountAsync();

        // ── 4. Sorting — use IOrderedQueryable so EF Core always emits ORDER BY ─
        var sortKey = filter.SortBy?.Trim().ToLowerInvariant();
        IOrderedQueryable<Listing> orderedQuery = sortKey switch
        {
            "priceasc"  => query.OrderBy(x => x.Price),
            "pricedesc" => query.OrderByDescending(x => x.Price),
            "areaasc"   => query.OrderBy(x => x.Area),
            "areadesc"  => query.OrderByDescending(x => x.Area),
            "oldest"    => query.OrderBy(x => x.CreatedAt).ThenBy(x => x.Id),
            _           => query.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id)
        };

        // ── 5. Pagination ────────────────────────────────────────────────────
        var skip = (pageNumber - 1) * pageSize;

        // ── 6. SQL-level projection — EF Core generates minimal SELECT ───────
        var items = await orderedQuery
            .Skip(skip)
            .Take(pageSize)
            .Select(x => new ListingDto
            {
                Id          = x.Id,
                Price       = x.Price,
                RoomCount   = x.RoomCount,
                Area        = x.Area,
                Floor       = x.Floor,
                TotalFloors = x.TotalFloors,
                ListingType  = x.ListingType,
                PropertyType = x.PropertyType,

                // Prefer requested lang, fall back to "az"
                Title = x.Translations
                    .Where(t => t.LanguageCode == lang)
                    .Select(t => t.Title)
                    .FirstOrDefault()
                    ?? x.Translations
                        .Where(t => t.LanguageCode == "az")
                        .Select(t => t.Title)
                        .FirstOrDefault()
                    ?? string.Empty,

                Description = x.Translations
                    .Where(t => t.LanguageCode == lang)
                    .Select(t => t.Description)
                    .FirstOrDefault()
                    ?? x.Translations
                        .Where(t => t.LanguageCode == "az")
                        .Select(t => t.Description)
                        .FirstOrDefault()
                    ?? string.Empty,

                CityName = x.City.Translations
                    .Where(t => t.LanguageCode == lang)
                    .Select(t => t.Name)
                    .FirstOrDefault()
                    ?? x.City.Translations
                        .Where(t => t.LanguageCode == "az")
                        .Select(t => t.Name)
                        .FirstOrDefault(),

                DistrictName = x.District != null
                    ? x.District.Translations
                        .Where(t => t.LanguageCode == lang)
                        .Select(t => t.Name)
                        .FirstOrDefault()
                      ?? x.District.Translations
                          .Where(t => t.LanguageCode == "az")
                          .Select(t => t.Name)
                          .FirstOrDefault()
                    : null,

                // Only the first image URL — no full Images entity load
                ThumbnailUrl = x.Images
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault()
            })
            .ToListAsync();

        return Result<PagedResult<ListingDto>>.Success(new PagedResult<ListingDto>
        {
            Items      = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize   = pageSize
        });
    }

    public async Task<Result<ListingDetailsDto>> GetByIdAsync(Guid id, string lang)
    {
        var listing = await _listingRepository.GetByIdWithDetailsAsync(id);

        if (listing == null)
            return Result<ListingDetailsDto>.Failure("Listing not found.");

        var translation = listing.Translations.FirstOrDefault(t => t.LanguageCode == lang)
                          ?? listing.Translations.FirstOrDefault(t => t.LanguageCode == "az");

        var dto = new ListingDetailsDto
        {
            Id = listing.Id,
            ListingType = listing.ListingType,
            RentType = listing.RentType,
            PropertyType = listing.PropertyType,
            SellerType = listing.SellerType,
            RepairStatus = listing.RepairStatus,
            Price = listing.Price,
            RoomCount = listing.RoomCount,
            Area = listing.Area,
            Floor = listing.Floor,
            TotalFloors = listing.TotalFloors,
            HasDocument = listing.HasDocument,
            HasMortgage = listing.HasMortgage,
            Name = listing.Name,
            Email = listing.Email,
            Phone = listing.Phone,
            Title = translation?.Title ?? string.Empty,
            Description = translation?.Description ?? string.Empty,
            CityName = GetTranslatedName(listing.City?.Translations, lang),
            DistrictName = GetTranslatedName(listing.District?.Translations, lang),
            MetroName = GetTranslatedName(listing.Metro?.Translations, lang),
            UserId = listing.UserId,
            UserFullName = listing.User?.FullName,
            ImageUrls = listing.Images?.Select(i => i.ImageUrl).ToList() ?? [],
            AgencyName = listing.Agency?.Name,
            ResidentialComplexName = GetTranslatedName(listing.ResidentialComplex?.Translations, lang)
        };

        if (listing.CommercialDetail != null)
            dto.CommercialDetail = _mapper.Map<ListingCommercialDetailDto>(listing.CommercialDetail);

        return Result<ListingDetailsDto>.Success(dto);
    }

    public async Task<Result<bool>> DeleteAsync(Guid id, Guid userId)
    {
        var listing = await _listingRepository.GetByIdAsync(id);

        if (listing == null)
            return Result<bool>.Failure("Listing not found.");

        if (listing.UserId != userId)
            return Result<bool>.Failure("You are not authorized to delete this listing.");

        // Delete images from wwwroot
        if (listing.Images != null)
        {
            foreach (var image in listing.Images)
            {
                _photoService.DeleteImage(image.ImageUrl);
            }
        }

        await _listingRepository.DeleteAsync(listing);

        return Result<bool>.Success(true);
    }

    private static string? GetTranslatedName<T>(IEnumerable<T>? translations, string lang)
        where T : class
    {
        if (translations == null) return null;

        var list = translations.ToList();
        var prop = typeof(T).GetProperty("LanguageCode");
        var nameProp = typeof(T).GetProperty("Name");

        if (prop == null || nameProp == null) return null;

        var match = list.FirstOrDefault(t => (string?)prop.GetValue(t) == lang)
                    ?? list.FirstOrDefault(t => (string?)prop.GetValue(t) == "az");

        return match != null ? (string?)nameProp.GetValue(match) : null;
    }

    private async Task AutoTranslateAsync(List<CreateListingTranslationDto> translations)
    {
        string[] allLangs = ["az", "en", "ru"];

        // Find the source translation (first one with non-empty title)
        var source = translations.FirstOrDefault(t => !string.IsNullOrWhiteSpace(t.Title));
        if (source == null) return;

        var sourceLang = source.LanguageCode;

        // Determine which languages are missing or empty
        var existingLangs = translations
            .Where(t => !string.IsNullOrWhiteSpace(t.Title))
            .Select(t => t.LanguageCode)
            .ToHashSet();

        var missingLangs = allLangs.Where(l => !existingLangs.Contains(l)).ToList();
        if (missingLangs.Count == 0) return;

        // Translate title and description in parallel
        var titleTranslations = await _translationService.TranslateToAllAsync(source.Title, sourceLang);
        var descTranslations = !string.IsNullOrWhiteSpace(source.Description)
            ? await _translationService.TranslateToAllAsync(source.Description, sourceLang)
            : new Dictionary<string, string>();

        foreach (var lang in missingLangs)
        {
            var existing = translations.FirstOrDefault(t => t.LanguageCode == lang);
            if (existing != null)
            {
                if (string.IsNullOrWhiteSpace(existing.Title))
                    existing.Title = titleTranslations.GetValueOrDefault(lang, source.Title);
                if (string.IsNullOrWhiteSpace(existing.Description))
                    existing.Description = descTranslations.GetValueOrDefault(lang, source.Description ?? string.Empty);
            }
            else
            {
                translations.Add(new CreateListingTranslationDto
                {
                    LanguageCode = lang,
                    Title = titleTranslations.GetValueOrDefault(lang, source.Title),
                    Description = descTranslations.GetValueOrDefault(lang, source.Description ?? string.Empty)
                });
            }
        }
    }

    }
