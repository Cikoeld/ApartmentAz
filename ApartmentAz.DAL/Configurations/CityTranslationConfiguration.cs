using ApartmentAz.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Configurations
{
    public class CityTranslationConfiguration : IEntityTypeConfiguration<CityTranslation>
    {
        public void Configure(EntityTypeBuilder<CityTranslation> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired();

            builder.HasOne<City>()
                .WithMany(x => x.Translations)
                .HasForeignKey(x => x.CityId);

            builder.HasIndex(x => new { x.CityId, x.LanguageCode })
                .IsUnique();
        }
    }
}
