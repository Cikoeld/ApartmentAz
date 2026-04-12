using ApartmentAz.BLL.Caching;
using ApartmentAz.BLL.DTOs.Listing;
using ApartmentAz.BLL.Interfaces;
using ApartmentAz.BLL.Services;
using ApartmentAz.DAL.Enums;
using ApartmentAz.DAL.Interfaces;
using ApartmentAz.DAL.Models;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace ApartmentAz.Tests.Services;

public class ListingServiceTests
{
    private readonly Mock<IListingRepository> _listingRepoMock;
    private readonly Mock<IPhotoService> _photoServiceMock;
    private readonly Mock<ITranslationService> _translationServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly IMemoryCache _cache;
    private readonly Mock<ICacheInvalidator> _cacheInvalidatorMock;
    private readonly ListingService _sut;

    public ListingServiceTests()
    {
        _listingRepoMock = new Mock<IListingRepository>();
        _photoServiceMock = new Mock<IPhotoService>();
        _translationServiceMock = new Mock<ITranslationService>();
        _mapperMock = new Mock<IMapper>();
        _cache = new MemoryCache(new MemoryCacheOptions());
        _cacheInvalidatorMock = new Mock<ICacheInvalidator>();

        _sut = new ListingService(
            _listingRepoMock.Object,
            _photoServiceMock.Object,
            _translationServiceMock.Object,
            _mapperMock.Object,
            _cache,
            _cacheInvalidatorMock.Object);
    }

    [Fact]
    public async Task CreateAsync_NullTranslations_ReturnsFailureAndDoesNotPersist()
    {
        // Arrange
        var dto = new CreateListingDto { Translations = null! };

        // Act
        var result = await _sut.CreateAsync(dto, Guid.NewGuid());

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Guid.Empty, result.Data);
        Assert.Contains("At least one translation is required", result.ErrorMessage!);
        _listingRepoMock.Verify(r => r.CreateAsync(It.IsAny<Listing>()), Times.Never);
        _cacheInvalidatorMock.Verify(c => c.InvalidateListings(), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_NoTranslations_ReturnsFailure()
    {
        // Arrange
        var dto = new CreateListingDto { Translations = [] };

        // Act
        var result = await _sut.CreateAsync(dto, Guid.NewGuid());

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(Guid.Empty, result.Data);
        Assert.Contains("translation", result.ErrorMessage!, StringComparison.OrdinalIgnoreCase);
        _listingRepoMock.Verify(r => r.CreateAsync(It.IsAny<Listing>()), Times.Never);
        _cacheInvalidatorMock.Verify(c => c.InvalidateListings(), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_CommercialWithoutDetail_ReturnsFailure()
    {
        // Arrange
        var dto = new CreateListingDto
        {
            PropertyType = PropertyType.Commercial,
            CommercialDetail = null,
            Translations = [new CreateListingTranslationDto
            {
                LanguageCode = "az",
                Title = "Test",
                Description = "Desc"
            }]
        };

        _translationServiceMock
            .Setup(t => t.TranslateToAllAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new Dictionary<string, string>
            {
                ["en"] = "Translated",
                ["ru"] = "Translated"
            });

        // Act
        var result = await _sut.CreateAsync(dto, Guid.NewGuid());

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("CommercialDetail", result.ErrorMessage!);
        _listingRepoMock.Verify(r => r.CreateAsync(It.IsAny<Listing>()), Times.Never);
        _cacheInvalidatorMock.Verify(c => c.InvalidateListings(), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_TranslationServiceThrows_DoesNotPersist()
    {
        // Arrange
        var dto = new CreateListingDto
        {
            PropertyType = PropertyType.NewBuilding,
            Translations =
            [
                new CreateListingTranslationDto
                {
                    LanguageCode = "az",
                    Title = "Title",
                    Description = "Description"
                }
            ]
        };

        _translationServiceMock
            .Setup(t => t.TranslateToAllAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new InvalidOperationException("Translation provider unavailable"));

        // Act + Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.CreateAsync(dto, Guid.NewGuid()));
        _listingRepoMock.Verify(r => r.CreateAsync(It.IsAny<Listing>()), Times.Never);
        _cacheInvalidatorMock.Verify(c => c.InvalidateListings(), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ValidDto_ReturnsSuccessAndInvalidatesCache()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var listingId = Guid.NewGuid();

        var dto = new CreateListingDto
        {
            PropertyType = PropertyType.NewBuilding,
            Translations = [new CreateListingTranslationDto
            {
                LanguageCode = "az",
                Title = "Test Listing",
                Description = "Description"
            }]
        };

        var listing = new Listing { Id = listingId };
        _mapperMock.Setup(m => m.Map<Listing>(dto)).Returns(listing);
        _mapperMock.Setup(m => m.Map<ListingTranslation>(It.IsAny<CreateListingTranslationDto>()))
            .Returns((CreateListingTranslationDto t) => new ListingTranslation
            {
                LanguageCode = t.LanguageCode,
                Title = t.Title,
                Description = t.Description
            });
        _listingRepoMock.Setup(r => r.CreateAsync(It.IsAny<Listing>())).Returns(Task.CompletedTask);
        _translationServiceMock
            .Setup(t => t.TranslateToAllAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new Dictionary<string, string>
            {
                ["en"] = "Translated EN",
                ["ru"] = "Translated RU"
            });

        // Act
        var result = await _sut.CreateAsync(dto, userId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Data);
        _listingRepoMock.Verify(r => r.CreateAsync(It.IsAny<Listing>()), Times.Once);
        _cacheInvalidatorMock.Verify(c => c.InvalidateListings(), Times.Once);
        _photoServiceMock.Verify(p => p.SaveImagesAsync(It.IsAny<List<Microsoft.AspNetCore.Http.IFormFile>>(), "listings"), Times.Never);
        _listingRepoMock.Verify(r => r.CreateAsync(It.Is<Listing>(l =>
            l.UserId == userId &&
            !l.IsApproved &&
            l.Translations.Count == 3 &&
            l.Translations.Any(t => t.LanguageCode == "az" && t.Title == "Test Listing") &&
            l.Translations.Any(t => t.LanguageCode == "en") &&
            l.Translations.Any(t => t.LanguageCode == "ru")
        )), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_ReturnsFailure()
    {
        // Arrange
        _listingRepoMock.Setup(r => r.GetByIdWithDetailsAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Listing?)null);

        // Act
        var result = await _sut.GetByIdAsync(Guid.NewGuid(), "az");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("not found", result.ErrorMessage!, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GetByIdAsync_Found_ReturnsSuccess()
    {
        // Arrange
        var listingId = Guid.NewGuid();
        var listing = new Listing
        {
            Id = listingId,
            Price = 100000,
            RoomCount = 3,
            Area = 80,
            Floor = 5,
            TotalFloors = 10,
            Name = "Test",
            Email = "test@test.com",
            Phone = "+994501234567",
            Translations = [new ListingTranslation
            {
                LanguageCode = "az",
                Title = "Test Elan",
                Description = "Açıqlama"
            }],
            Images =
            [
                new ListingImage
                {
                    Id = Guid.NewGuid(),
                    ListingId = listingId,
                    ImageUrl = "https://cdn.example.com/1.jpg"
                }
            ],
            City = new City
            {
                Translations = [new CityTranslation { LanguageCode = "az", Name = "Bakı" }]
            },
            User = new AppUser { FullName = "Test User" }
        };

        _listingRepoMock.Setup(r => r.GetByIdWithDetailsAsync(listingId))
            .ReturnsAsync(listing);

        // Act
        var result = await _sut.GetByIdAsync(listingId, "az");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Test Elan", result.Data!.Title);
        Assert.Equal(100000, result.Data.Price);
        Assert.Equal("Açıqlama", result.Data.Description);
        Assert.Equal("Bakı", result.Data.CityName);
        Assert.Equal("Test User", result.Data.UserFullName);
        Assert.Single(result.Data.ImageUrls);
        Assert.Equal("https://cdn.example.com/1.jpg", result.Data.ImageUrls[0]);
    }

    [Fact]
    public async Task DeleteAsync_NotFound_ReturnsFailureAndDoesNotInvalidateCache()
    {
        // Arrange
        _listingRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Listing?)null);

        // Act
        var result = await _sut.DeleteAsync(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.False(result.IsSuccess);
        Assert.False(result.Data);
        Assert.Contains("not found", result.ErrorMessage!, StringComparison.OrdinalIgnoreCase);
        _listingRepoMock.Verify(r => r.DeleteAsync(It.IsAny<Listing>()), Times.Never);
        _photoServiceMock.Verify(p => p.DeleteImage(It.IsAny<string>()), Times.Never);
        _cacheInvalidatorMock.Verify(c => c.InvalidateListings(), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_NotOwner_ReturnsFailure()
    {
        // Arrange
        var listingId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();
        var otherId = Guid.NewGuid();

        _listingRepoMock.Setup(r => r.GetByIdAsync(listingId))
            .ReturnsAsync(new Listing { Id = listingId, UserId = ownerId });

        // Act
        var result = await _sut.DeleteAsync(listingId, otherId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("not authorized", result.ErrorMessage!, StringComparison.OrdinalIgnoreCase);
        _listingRepoMock.Verify(r => r.DeleteAsync(It.IsAny<Listing>()), Times.Never);
        _photoServiceMock.Verify(p => p.DeleteImage(It.IsAny<string>()), Times.Never);
        _cacheInvalidatorMock.Verify(c => c.InvalidateListings(), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_Owner_ReturnsSuccessAndInvalidatesCache()
    {
        // Arrange
        var listingId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _listingRepoMock.Setup(r => r.GetByIdAsync(listingId))
            .ReturnsAsync(new Listing { Id = listingId, UserId = userId, Images = [] });
        _listingRepoMock.Setup(r => r.DeleteAsync(It.IsAny<Listing>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _sut.DeleteAsync(listingId, userId);

        // Assert
        Assert.True(result.IsSuccess);
        _listingRepoMock.Verify(r => r.DeleteAsync(It.IsAny<Listing>()), Times.Once);
        _cacheInvalidatorMock.Verify(c => c.InvalidateListings(), Times.Once);
    }
}
