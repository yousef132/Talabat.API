using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Spepcifications;

namespace Talabat.Core.Specifications.OrderSpecifications
{
    public class OrderWithPaymentIntentSpecification : BaseSpecification<Order>
    {

        public OrderWithPaymentIntentSpecification(string? paymentIntentId)
            : base(o => o.PaymentIntentId == paymentIntentId)
        {

        }
    }
}
