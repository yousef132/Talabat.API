﻿namespace Talabat.Core.Entities.Order_Aggregate
{
    public class ProductItemOrdered
    {
        // just for EF CORE
        private ProductItemOrdered()
        {

        }
        public ProductItemOrdered(int productId, string productName, string pictureUrl)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
    }
}
