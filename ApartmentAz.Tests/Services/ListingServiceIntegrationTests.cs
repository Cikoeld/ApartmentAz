using ApartmentAz.BLL.Caching;
using ApartmentAz.BLL.DTOs.Listing;
using ApartmentAz.BLL.Interfaces;
using ApartmentAz.BLL.Services;
using ApartmentAz.DAL.Data;
using ApartmentAz.DAL.Enums;
using ApartmentAz.DAL.Models;
using ApartmentAz.DAL.Repositories;
using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace ApartmentAz.Tests.Services;

public class ListingServiceIntegrationTests
{
    private static IMapper CreateMapper()
    {
        var mapperMock = new Mock<IMapper>();

        mapperMock
            .Setup(m => m.Map<Listing>(It.IsAny<CreateListingDto>()))
            .Returns((CreateListingDto dto) => new Listing
            {
                ListingType = dto.ListingType,
                RentType = dto.RentType,
                PropertyType = dto.PropertyType,
                SellerType = dto.SellerType,
                RepairStatus = dto.RepairStatus,
                Price = dto.Price,
                RoomCount = dto.RoomCount,
                Area = dto.Area,
                Floor = dto.Floor,
                TotalFloors = dto.TotalFloors,
                HasDocument = dto.HasDocument,
                HasMortgage = dto.HasMortgage,
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                CityId = dto.CityId,
                DistrictId = dto.DistrictId,
                MetroId = dto.MetroId,
                AgencyId = dto.AgencyId,
                ResidentialComplexId = dto.ResidentialComplexId
            });

        mapperMock
            .Setup(m => m.Map<ListingTranslation>(It.IsAny<CreateListingTranslationDto>()))
            .Returns((CreateListingTranslationDto translation) => new ListingTranslation
            {
                LanguageCode = translation.LanguageCode,
                Title = translation.Title,
                Description = translation.Description
            });

        return mapperMock.Object;
    }

    private static AppDbContext CreateContext(SqliteConnection connection)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    private static AppUser CreateUser(Guid userId)
    {
        return new AppUser
        {
            Id = userId,
            FullName = "Integration User",
            Email = "integration@test.com",
            UserName = "integration@test.com",
            NormalizedEmail = "INTEGRATION@TEST.COM",
            NormalizedUserName = "INTEGRATION@TEST.COM",
            SecurityStamp = Guid.NewGuid().ToString("N")
        };
    }

    [Fact]
    public async Task CreateAsync_RealDb_PersistsListingGraph()
    {
        // Arrange
        using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var context = CreateContext(connection);

        var userId = Guid.NewGuid();
        var cityId = Guid.NewGuid();
        context.Users.Add(CreateUser(userId));
        context.Cities.Add(new City
        {
            Id = cityId,
            Translations =
            [
                new CityTranslation
                {
                    Id = Guid.NewGuid(),
                    CityId = cityId,
                    LanguageCode = "az",
                    Name = "Bakı"
                }
            ]
        });
        await context.SaveChangesAsync();

        var repo = new ListingRepository(context);
        var photoServiceMock = new Mock<IPhotoService>();
        var translationServiceMock = new Mock<ITranslationService>();
        var cacheInvalidatorMock = new Mock<ICacheInvalidator>();

        translationServiceMock
            .Setup(t => t.TranslateToAllAsync(It.IsAny<string>(), "az"))
            .ReturnsAsync(new Dictionary<string, string>
            {
                ["en"] = "English",
                ["ru"] = "Russian"
            });

        var service = new ListingService(
            repo,
            photoServiceMock.Object,
            translationServiceMock.Object,
            CreateMapper(),
            new MemoryCache(new MemoryCacheOptions()),
            cacheInvalidatorMock.Object);

        var dto = new CreateListingDto
        {
            ListingType = ListingType.Sale,
            PropertyType = PropertyType.NewBuilding,
            SellerType = SellerType.Owner,
            RepairStatus = RepairStatus.Repaired,
            Price = 200000,
            RoomCount = 3,
            Area = 95,
            Floor = 7,
            TotalFloors = 16,
            HasDocument = true,
            HasMortgage = false,
            Name = "Owner",
            Email = "owner@test.com",
            Phone = "+994501112233",
            CityId = cityId,
            Translations =
            [
                new CreateListingTranslationDto
                {
                    LanguageCode = "az",
                    Title = "Yeni mənzil",
                    Description = "Təsvir"
                }
            ]
        };

        // Act
        var result = await service.CreateAsync(dto, userId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Data);

        var listing = await context.Listings
            .Include(x => x.Translations)
            .SingleAsync(x => x.Id == result.Data);

        Assert.Equal(userId, listing.UserId);
        Assert.False(listing.IsApproved);
        Assert.Equal(cityId, listing.CityId);
        Assert.InRange(listing.CreatedAt, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow.AddMinutes(1));
        Assert.Equal(3, listing.Translations.Count);
        Assert.Contains(listing.Translations, t => t.LanguageCode == "az" && t.Title == "Yeni mənzil");
        Assert.Contains(listing.Translations, t => t.LanguageCode == "en" && t.Title == "English");
        Assert.Contains(listing.Translations, t => t.LanguageCode == "ru" && t.Title == "Russian");
        cacheInvalidatorMock.Verify(c => c.InvalidateListings(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_RealDb_FallsBackToAzTranslationWhenLanguageMissing()
    {
        // Arrange
        using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var context = CreateContext(connection);

        var userId = Guid.NewGuid();
        var cityId = Guid.NewGuid();
        var listingId = Guid.NewGuid();

        context.Users.Add(CreateUser(userId));
        context.Cities.Add(new City
        {
            Id = cityId,
            Translations =
            [
                new CityTranslation
                {
                    Id = Guid.NewGuid(),
                    CityId = cityId,
                    LanguageCode = "az",
                    Name = "Bakı"
                }
            ]
        });

        context.Listings.Add(new Listing
        {
            Id = listingId,
            UserId = userId,
            ListingType = ListingType.Sale,
            PropertyType = PropertyType.NewBuilding,
            SellerType = SellerType.Owner,
            RepairStatus = RepairStatus.Repaired,
            Price = 150000,
            RoomCount = 2,
            Area = 70,
            Floor = 2,
            TotalFloors = 9,
            HasDocument = true,
            HasMortgage = false,
            Name = "Owner",
            Email = "owner@test.com",
            Phone = "+994501112233",
            CityId = cityId,
            CreatedAt = DateTime.UtcNow,
            IsApproved = true,
            Translations =
            [
                new ListingTranslation
                {
                    Id = Guid.NewGuid(),
                    ListingId = listingId,
                    LanguageCode = "az",
                    Title = "Yalnız az",
                    Description = "Açıqlama"
                }
            ]
        });

        await context.SaveChangesAsync();

        var service = new ListingService(
            new ListingRepository(context),
            Mock.Of<IPhotoService>(),
            Mock.Of<ITranslationService>(),
            CreateMapper(),
            new MemoryCache(new MemoryCacheOptions()),
            Mock.Of<ICacheInvalidator>());

        // Act
        var result = await service.GetByIdAsync(listingId, "en");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal("Yalnız az", result.Data.Title);
        Assert.Equal("Açıqlama", result.Data.Description);
        Assert.Equal("Bakı", result.Data.CityName);
    }

    [Fact]
    public async Task DeleteAsync_RealDb_DeletesListingAndStoredImages()
    {
        // Arrange
        using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await using var context = CreateContext(connection);

        var userId = Guid.NewGuid();
        var cityId = Guid.NewGuid();
        var listingId = Guid.NewGuid();

        context.Users.Add(CreateUser(userId));
        context.Cities.Add(new City
        {
            Id = cityId,
            Translations =
            [
                new CityTranslation
                {
                    Id = Guid.NewGuid(),
                    CityId = cityId,
                    LanguageCode = "az",
                    Name = "Bakı"
                }
            ]
        });

        context.Listings.Add(new Listing
        {
            Id = listingId,
            UserId = userId,
            ListingType = ListingType.Sale,
            PropertyType = PropertyType.NewBuilding,
            SellerType = SellerType.Owner,
            RepairStatus = RepairStatus.Repaired,
            Price = 180000,
            RoomCount = 3,
            Area = 90,
            Floor = 4,
            TotalFloors = 14,
            HasDocument = true,
            HasMortgage = false,
            Name = "Owner",
            Email = "owner@test.com",
            Phone = "+994501112233",
            CityId = cityId,
            CreatedAt = DateTime.UtcNow,
            IsApproved = true,
            Images =
            [
                new ListingImage { Id = Guid.NewGuid(), ListingId = listingId, ImageUrl = "img-1.jpg" },
                new ListingImage { Id = Guid.NewGuid(), ListingId = listingId, ImageUrl = "img-2.jpg" }
            ]
        });

        await context.SaveChangesAsync();

        var photoServiceMock = new Mock<IPhotoService>();
        var cacheInvalidatorMock = new Mock<ICacheInvalidator>();

        var service = new ListingService(
            new ListingRepository(context),
            photoServiceMock.Object,
            Mock.Of<ITranslationService>(),
            CreateMapper(),
            new MemoryCache(new MemoryCacheOptions()),
            cacheInvalidatorMock.Object);

        // Act
        var result = await service.DeleteAsync(listingId, userId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Data);
        Assert.False(await context.Listings.AnyAsync(x => x.Id == listingId));
        photoServiceMock.Verify(p => p.DeleteImage("img-1.jpg"), Times.Once);
        photoServiceMock.Verify(p => p.DeleteImage("img-2.jpg"), Times.Once);
        cacheInvalidatorMock.Verify(c => c.InvalidateListings(), Times.Once);
    }
}
