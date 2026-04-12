using ApartmentAz.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApartmentAz.DAL.Data
{
    public class AppDbContext
        : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // 🏠 Listings
        public DbSet<Listing> Listings { get; set; }
        public DbSet<ListingTranslation> ListingTranslations { get; set; }
        public DbSet<ListingImage> ListingImages { get; set; }

        // 📍 Location
        public DbSet<City> Cities { get; set; }
        public DbSet<CityTranslation> CityTranslations { get; set; }

        public DbSet<District> Districts { get; set; }
        public DbSet<DistrictTranslation> DistrictTranslations { get; set; }

        public DbSet<Metro> Metros { get; set; }
        public DbSet<MetroTranslation> MetroTranslations { get; set; }

        public DbSet<Agency> Agencies { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        public DbSet<CommercialDetail> CommercialDetails { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<ResidentialComplex> ResidentialComplexes { get; set; }
        public DbSet<ResidentialComplexTranslation> ResidentialComplexTranslations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // 🔐 Identity default config
            base.OnModelCreating(builder);

            // 🔥 BÜTÜN Fluent API Configuration-ları buradan yüklənir
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}