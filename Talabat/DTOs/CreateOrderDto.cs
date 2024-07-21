using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class CreateOrderDto
    {
        [Required]
        public string BuyerEmail { get; set; }
        [Required]

        public string CartId { get; set; }
        [Required]

        public int DeliveryMethodId { get; set; }
        [Required]
        public OrderAddressDto Address { get; set; }
    }
}
