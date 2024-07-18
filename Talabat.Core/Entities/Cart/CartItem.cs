namespace Talabat.Core.Entities.Cart
{
    // product selected as an item in the cart with specific quantity
    public class CartItem
    {
        // id of product and  at the same time item id
        public int Id { get; set; }

        public string Name { get; set; }

        public string PictureUrl { get; set; }

        // not the product price but price of the product when added to the cart (if there is discount or something like this)
        public decimal Price { get; set; }

        public int Quantity { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
    }
}
