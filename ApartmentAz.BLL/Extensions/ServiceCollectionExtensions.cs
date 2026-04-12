using ApartmentAz.BLL.Caching;
using ApartmentAz.BLL.Interfaces;
using ApartmentAz.BLL.MappingProfiles;
using ApartmentAz.BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ApartmentAz.BLL.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBllServices(this IServiceCollection services)
    {
        // In-memory cache (used by TranslationService)
        services.AddMemoryCache();

        // Cache invalidator
        services.AddSingleton<ICacheInvalidator, CacheInvalidator>();

        // AutoMapper
        services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

        // Photo (wwwroot)
        services.AddScoped<IPhotoService, PhotoService>();

        // Auth & User
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailService, SmtpEmailService>();
        services.AddScoped<IUserService, UserService>();

        // Listings
        services.AddScoped<IListingService, ListingService>();
        services.AddScoped<IAdminService, AdminService>();

        // Location (separate services)
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<IDistrictService, DistrictService>();
        services.AddScoped<IMetroService, MetroService>();

        // Favorite
        services.AddScoped<IFavoriteService, FavoriteService>();

        // Agency
        services.AddScoped<IAgencyService, AgencyService>();

        // Residential Complex
        services.AddScoped<IResidentialComplexService, ResidentialComplexService>();

        // Chat
        services.AddScoped<IChatService, ChatService>();

        // Translation (MyMemory API)
        services.AddHttpClient<ITranslationService, TranslationService>(client =>
        {
            client.BaseAddress = new Uri("https://api.mymemory.translated.net/");
            client.Timeout = TimeSpan.FromSeconds(10);
        });

        return services;
    }
}
