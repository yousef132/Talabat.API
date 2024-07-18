﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities;

namespace Talabat.Infrastructure.Data.Configurations
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Description).IsRequired();
            builder.Property(p => p.PictureUrl).IsRequired();

            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");

            builder.HasOne(p => p.Brand)
                .WithMany()
                .HasForeignKey(p => p.BrandId);

            builder.HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId);
        }
    }
}