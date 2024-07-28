using Talabat.Core.Entities.Cart;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Services.Contract
{
    public interface IPaymentService
    {
        Task<CustomerCart?> CreateOrUpdatePaymentIntent(string cartId);

        Task<Order?> UpdateOrderStatus(string paymentIntentId, bool isPaid);
    }
}
