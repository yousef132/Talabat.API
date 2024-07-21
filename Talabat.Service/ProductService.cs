using Talabat.Core;
using Talabat.Core.Entities.Product_Aggregate;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.ProductsSpecifications;
using Talabat.Core.Spepcifications.ProductsSpecifications;

namespace Talabat.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
        => await unitOfWork.Repository<ProductBrand>().GetAllAsync();


        public async Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync()
        => await unitOfWork.Repository<ProductCategory>().GetAllAsync();



        public Task<int> GetCountAsync(ProductSpecificationsParams specsParams)
        {
            var countSpecs = new ProductWithFiltrationForCountSpecifications(specsParams);
            var count = unitOfWork.Repository<Product>().GetCountAsync(countSpecs);
            return count;
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var specs = new ProductWithBrandAndCategorySpecifications(id);
            var product = await unitOfWork.Repository<Product>().GetWithSpecificationAsync(specs);
            return product;
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecificationsParams specsParams)
        {
            var specs = new ProductWithBrandAndCategorySpecifications(specsParams);
            var products = await unitOfWork.Repository<Product>().GetAllWithSpecificationAsync(specs);
            return products;
        }
    }
}
