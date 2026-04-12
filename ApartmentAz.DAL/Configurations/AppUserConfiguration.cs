using ApartmentAz.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(100);

        // əlavə etmək istəsən:
        builder.HasMany(x => x.Listings)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
    }
}