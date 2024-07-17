using Talabat.Core.Entities;

namespace Talabat.Core.Spepcifications.ProductsSpecifications
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecification<Product>
    {
        // this ctor will be used for creating an object that will be used to get all products 
        public ProductWithBrandAndCategorySpecifications(ProductSpecificationsParams specsParams)
            : base(p =>
                 (specsParams.BrandId == null || p.BrandId == specsParams.BrandId)
                  && (specsParams.CategoryId == null || p.CategoryId == specsParams.CategoryId)
            && (string.IsNullOrEmpty(specsParams.ProductName) || p.Name.ToLower().Contains(specsParams.ProductName))
            )
        {
            AddInclude(p => p.Brand);
            AddInclude(p => p.Category);

            if (!string.IsNullOrEmpty(specsParams.Sort))
            {
                switch (specsParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;

                    case "priceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;

                    default:
                        AddOrderBy(p => p.Name);
                        break;

                }

            }
            else
                AddOrderBy(p => p.Name);

            ApplyPagination((specsParams.PageIndex - 1) * specsParams.PageSize, specsParams.PageSize);
        }
        public ProductWithBrandAndCategorySpecifications(int id)
            : base(p => p.Id == id)
        {
            AddInclude(p => p.Brand);
            AddInclude(p => p.Category);
        }
    }
}
