using ApartmentAz.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Configurations
{
    public class ResidentialComplexConfiguration : IEntityTypeConfiguration<ResidentialComplex>
    {
        public void Configure(EntityTypeBuilder<ResidentialComplex> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Listings)
                .WithOne(x => x.ResidentialComplex)
                .HasForeignKey(x => x.ResidentialComplexId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
