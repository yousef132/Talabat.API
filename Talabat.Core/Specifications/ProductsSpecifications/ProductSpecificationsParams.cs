namespace Talabat.Core.Spepcifications.ProductsSpecifications
{
    public class ProductSpecificationsParams
    {
        public string? Sort { get; set; }
        private string productName;

        public string? ProductName
        {
            get { return productName; }
            set { productName = value.ToLower(); }
        }

        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }

        private const int MAXPAGESIZE = 10;
        private int _pageSize = 5;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > MAXPAGESIZE ? MAXPAGESIZE : value; }
        }

        public int PageIndex { get; set; } = 1;

    }
}
