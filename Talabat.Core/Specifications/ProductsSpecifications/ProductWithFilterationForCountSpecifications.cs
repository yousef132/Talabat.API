using Talabat.Core.Entities;
using Talabat.Core.Spepcifications;
using Talabat.Core.Spepcifications.ProductsSpecifications;

namespace Talabat.Core.Specifications.ProductsSpecifications
{
    public class ProductWithFiltrationForCountSpecifications : BaseSpecification<Product>
    {
        public ProductWithFiltrationForCountSpecifications(ProductSpecificationsParams specsParams)
            : base(p =>
                 (specsParams.BrandId == null || p.BrandId == specsParams.BrandId)
                  && (specsParams.CategoryId == null || p.CategoryId == specsParams.CategoryId)
                  && (string.IsNullOrEmpty(specsParams.ProductName) || p.Name.ToLower().Contains(specsParams.ProductName))
            )
        {

        }
    }
}
