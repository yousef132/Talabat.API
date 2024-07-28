using Talabat.Core;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Entities.Product_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.OrderSpecifications;

namespace Talabat.Service.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly ICartRepository cartRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPaymentService paymentService;

        public OrderService(
             ICartRepository cartRepository,
             IUnitOfWork unitOfWork,
             IPaymentService paymentService

            )
        {
            this.cartRepository = cartRepository;
            this.unitOfWork = unitOfWork;
            this.paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string cartId, Address shippingAddress, string buyerEmail, int deliveryMethodId)
        {
            var cart = await cartRepository.GetCartAsync(cartId);
            var orderItems = new List<OrderItem>();
            if (cart?.Items.Count > 0)
            {
                // iterate over cart items
                foreach (var item in cart.Items)
                {
                    var product = await unitOfWork.Repository<Product>().GetAsync(item.Id);
                    var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);

                    var orderItem = new OrderItem(productItemOrdered, item.Quantity, item.Price);

                    orderItems.Add(orderItem);
                }
            }

            // calculate subtotal

            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);


            // get delivery method

            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetAsync(deliveryMethodId);

            var orderRepo = unitOfWork.Repository<Order>();


            var spec = new OrderWithPaymentIntentSpecification(cart?.PaymentIntentId);
            var existingOrder = await orderRepo.GetWithSpecificationAsync(spec);

            if (existingOrder != null)
            {
                orderRepo.Delete(existingOrder);
                await paymentService.CreateOrUpdatePaymentIntent(cartId);

            }


            var order = new Order(buyerEmail, shippingAddress, deliveryMethodId, deliveryMethod, orderItems, subTotal, cart.PaymentIntentId);

            await orderRepo.Add(order);
            var result = await unitOfWork.CompleteAsync();
            if (result <= 0)
                return null;


            return order;

        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
              => await unitOfWork.Repository<DeliveryMethod>().GetAllAsync();


        public async Task<Order?> GetOrderByIdForUser(string buyerEmail, int orderId)
        {

            var orderSpecs = new OrdersSpecs(buyerEmail, orderId);
            var orders = await unitOfWork.Repository<Order>().GetWithSpecificationAsync(orderSpecs);
            return orders;

        }

        public async Task<IReadOnlyList<Order>> GetOrdersOfUserAsync(string buyerEmail)
        {

            var orderSpecs = new OrdersSpecs(buyerEmail);
            var orders = await unitOfWork.Repository<Order>().GetAllWithSpecificationAsync(orderSpecs);

            return orders;
        }
    }
}
