using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class CustomerCartDto
    {
        [Required]
        public string Id { get; set; }
        [Required]

        public List<CartItemDto> Items { get; set; }

        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }

        public int? DeliveryMethodId { get; set; }

        public decimal ShippingPrice { get; set; }
    }
}
