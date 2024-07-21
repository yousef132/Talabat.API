using System.ComponentModel.DataAnnotations.Schema;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class Order : BaseEntity
    {

        private Order()
        {

        }

        public Order(string buyerEmail, Address shippingAddress, int? deliveryMethodId, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethodId = deliveryMethodId;
            Items = items;
            SubTotal = subTotal;
            DeliveryMethod = deliveryMethod;
        }

        public string BuyerEmail { get; set; }
        // Greenwich Time 
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;


        // 1:1 T:T  so they will be mapped in the same table (Order own an address)
        public Address ShippingAddress { get; set; }
        public int? DeliveryMethodId { get; set; }
        [ForeignKey(nameof(DeliveryMethodId))]
        public DeliveryMethod? DeliveryMethod { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        // sum(cost*quantity)
        public decimal SubTotal { get; set; }
        public decimal GetTotal() => SubTotal + DeliveryMethod.Cost;
        public string PaymentIntentId { get; set; } = string.Empty;
    }


}
