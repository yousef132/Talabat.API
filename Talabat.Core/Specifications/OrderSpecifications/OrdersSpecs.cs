using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Spepcifications;

namespace Talabat.Core.Specifications.OrderSpecifications
{
    public class OrdersSpecs : BaseSpecification<Order>
    {
        // for getting all orders for specific user 
        public OrdersSpecs(string email)
            : base(o => o.BuyerEmail == email)

        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.Items);
            AddOrderByDesc(o => o.OrderDate);
        }

        public OrdersSpecs(string email, int orderId)
            : base(o => o.Id == orderId && o.BuyerEmail == email)
        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.Items);
        }

    }
}
