using ApartmentAz.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Configurations
{
    public class MetroConfiguration : IEntityTypeConfiguration<Metro>
    {
        public void Configure(EntityTypeBuilder<Metro> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.City)
                .WithMany(x => x.Metros)
                .HasForeignKey(x => x.CityId);
        }
    }
}
