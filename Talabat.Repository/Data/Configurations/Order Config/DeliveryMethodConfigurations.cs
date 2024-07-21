﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Infrastructure.Data.Configurations.Order_Config
{
    internal class DeliveryMethodConfigurations : IEntityTypeConfiguration<DeliveryMethod>
    {

        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {

            builder.Property(dm => dm.Cost)
                .HasColumnType("decimal(12,2)");

        }
    }

}
