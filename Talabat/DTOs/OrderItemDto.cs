namespace Talabat.APIs.DTOs
{
    public class OrderItemDto
    {

        // item id
        public int Id { get; set; } 
        // product id as an item in the order
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public decimal Price {  get; set; }

        public int Quantity { get; set; }   

        public string PictureUrl { get; set; }




    }
}
