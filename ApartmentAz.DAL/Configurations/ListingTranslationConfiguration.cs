using ApartmentAz.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Configurations
{
    public class ListingTranslationConfiguration : IEntityTypeConfiguration<ListingTranslation>
    {
        public void Configure(EntityTypeBuilder<ListingTranslation> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).IsRequired();

            builder.HasOne(x => x.Listing)
                .WithMany(x => x.Translations)
                .HasForeignKey(x => x.ListingId);

            builder.HasIndex(x => new { x.ListingId, x.LanguageCode })
                .IsUnique();
        }
    }
}
