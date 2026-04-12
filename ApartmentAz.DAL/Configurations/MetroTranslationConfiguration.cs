using ApartmentAz.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Configurations
{
    public class MetroTranslationConfiguration : IEntityTypeConfiguration<MetroTranslation>
    {
        public void Configure(EntityTypeBuilder<MetroTranslation> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired();

            builder.HasOne<Metro>()
                .WithMany(x => x.Translations)
                .HasForeignKey(x => x.MetroId);

            builder.HasIndex(x => new { x.MetroId, x.LanguageCode })
                .IsUnique();
        }
    }
}
