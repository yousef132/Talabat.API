using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class CustomerCartDto
    {
        [Required]
        public string Id { get; set; }
        [Required]

        public List<CartItemDto> Items { get; set; }
    }
}
