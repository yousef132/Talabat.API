
using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class CartItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]

        public string Name { get; set; }
        [Required]

        public string PictureUrl { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than one.")]

        public int Quantity { get; set; }
        [Required]

        public string Category { get; set; }
        [Required]

        public string Brand { get; set; }
    }
}