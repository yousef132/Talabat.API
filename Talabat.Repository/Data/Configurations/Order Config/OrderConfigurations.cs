using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Infrastructure.Data.Configurations.Order_Config
{
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
    {

        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, shippingAddress => shippingAddress.WithOwner());

            builder.Property(o => o.Status)
                .HasConversion(
                    (Ostatus) => Ostatus.ToString(),
                    (Ostatus) => (OrderStatus)Enum.Parse(typeof(OrderStatus), Ostatus)
                    );

            builder.Property(o => o.SubTotal)
                .HasColumnType("decimal(12,2)");

            builder.HasOne(o => o.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(o => o.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

        }
    }

}
