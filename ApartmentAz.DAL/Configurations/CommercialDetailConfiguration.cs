using ApartmentAz.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApartmentAz.DAL.Configurations
{
    public class CommercialDetailConfiguration : IEntityTypeConfiguration<CommercialDetail>
    {
        public void Configure(EntityTypeBuilder<CommercialDetail> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Listing)
                .WithOne(x => x.CommercialDetail)
                .HasForeignKey<CommercialDetail>(x => x.ListingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
