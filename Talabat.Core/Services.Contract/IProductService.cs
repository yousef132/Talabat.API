using Talabat.Core.Entities.Product_Aggregate;
using Talabat.Core.Spepcifications.ProductsSpecifications;

namespace Talabat.Core.Services.Contract
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecificationsParams specsParams);

        Task<Product?> GetProductByIdAsync(int id);

        Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();
        Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync();

        Task<int> GetCountAsync(ProductSpecificationsParams specsParams);
    }
}
