using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Infrastructure.Data.Configurations.Order_Config
{
    internal class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {

        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(oi => oi.Product)
                 .WithOwner();

            builder.Property(o => o.Price)
                .HasColumnType("decimal(12,2)");

        }
    }

}
