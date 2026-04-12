using ApartmentAz.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Configurations
{
    public class DistrictTranslationConfiguration : IEntityTypeConfiguration<DistrictTranslation>
    {
        public void Configure(EntityTypeBuilder<DistrictTranslation> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired();

            builder.HasOne<District>()
                .WithMany(x => x.Translations)
                .HasForeignKey(x => x.DistrictId);

            builder.HasIndex(x => new { x.DistrictId, x.LanguageCode })
                .IsUnique();
        }
    }
}
