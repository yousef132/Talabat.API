using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Services.Contract
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string cartId, Address shippingAddress, string buyerEmail, int DeliveryMethodId);
        Task<IReadOnlyList<Order>> GetOrdersOfUserAsync(string buyerEmail);
        Task<Order?> GetOrderByIdForUser(string buyerEmail, int orderId);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}
