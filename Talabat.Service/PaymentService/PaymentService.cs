using Microsoft.Extensions.Configuration;
using Stripe;
using Talabat.Core;
using Talabat.Core.Entities.Cart;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.OrderSpecifications;
using Product = Talabat.Core.Entities.Product_Aggregate.Product;
namespace Talabat.Service.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration configuration;
        private readonly ICartRepository cartRepository;
        private readonly IUnitOfWork unitOfWork;

        public PaymentService(IConfiguration configuration,
            ICartRepository cartRepository,
            IUnitOfWork unitOfWork)
        {
            this.configuration = configuration;
            this.cartRepository = cartRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<CustomerCart?> CreateOrUpdatePaymentIntent(string cartId)
        {
            // setting Secret Key
            StripeConfiguration.ApiKey = configuration["Stripe:Secretkey"];

            // creating payment intent with options (amount...) 
            // so get cart products and calculate amount

            var cart = await cartRepository.GetCartAsync(cartId);

            if (cart == null)
                return null;

            decimal shippingCost = 0m;
            if (cart.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetAsync(cart.DeliveryMethodId.Value);
                shippingCost = deliveryMethod.Cost;
                cart.ShippingPrice = shippingCost;
            }
            if (cart.Items?.Count() > 0)
            {
                var productRepository = unitOfWork.Repository<Product>();

                foreach (var item in cart.Items)
                {
                    // get product from db to ensure the price
                    var product = await productRepository.GetAsync(item.Id);


                    if (item.Price != product?.Price)
                        item.Price = product.Price;

                }
            }


            PaymentIntent paymentIntent;
            PaymentIntentService paymentIntentService = new PaymentIntentService();

            // create new payment intent (cuz PaymentIntentId is null)
            if (string.IsNullOrEmpty(cart.PaymentIntentId))
            {
                // generation options
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)cart.Items.Sum(i => i.Price * i.Quantity * 100) + (long)shippingCost * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };


                // integration with stripe
                paymentIntent = await paymentIntentService.CreateAsync(options);

                cart.PaymentIntentId = paymentIntent.Id;
                cart.ClientSecret = paymentIntent.ClientSecret;


            }
            else
            {
                // update existing Payment Intent 
                // generating new options with new amount
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)cart.Items.Sum(i => i.Price * i.Quantity * 100) + (long)shippingCost * 100
                };

                paymentIntent = await paymentIntentService.UpdateAsync(cart.PaymentIntentId, options);
            }

            // updating cart 

            await cartRepository.UpdateCartAsync(cart);
            return cart;
        }

        public async Task<Order?> UpdateOrderStatus(string paymentIntentId, bool isPaid)
        {
            var spec = new OrderWithPaymentIntentSpecification(paymentIntentId);
            var order = await unitOfWork.Repository<Order>().GetWithSpecificationAsync(spec);

            if (order == null)
                return null;

            if (isPaid)
                order.Status = OrderStatus.PaymentReceived;
            order.Status = OrderStatus.PaymentFailed;

            unitOfWork.Repository<Order>().Update(order);
            await unitOfWork.CompleteAsync();
            return order;
        }
    }
}
