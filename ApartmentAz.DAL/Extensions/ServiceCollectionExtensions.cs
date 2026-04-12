using ApartmentAz.DAL.Interfaces;
using ApartmentAz.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ApartmentAz.DAL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDalRepositories(this IServiceCollection services)
        {
            // 🔥 Generic Repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // 📍 Location
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IDistrictRepository, DistrictRepository>();
            services.AddScoped<IMetroRepository, MetroRepository>();

            // 🏠 Listings
            services.AddScoped<IListingRepository, ListingRepository>();

            // 👤 User (əgər istifadə edirsənsə)
            services.AddScoped<IUserRepository, UserRepository>();

            // ❤️ Favorite (opsional amma yaxşı feature)
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();

            // 🏢 Agency
            services.AddScoped<IAgencyRepository, AgencyRepository>();

            // 💬 Messages
            services.AddScoped<IMessageRepository, MessageRepository>();

            // 🔑 Refresh Tokens
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            // 🏘️ Residential Complex
            services.AddScoped<IResidentialComplexRepository, ResidentialComplexRepository>();

            return services;
        }
    }
}