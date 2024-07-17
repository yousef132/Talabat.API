using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications.ProductsSpecifications;
using Talabat.Core.Spepcifications.ProductsSpecifications;

namespace Talabat.APIs.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IGenericRepository<Product> productRepository;
        private readonly IMapper mapper;
        private readonly IGenericRepository<ProductCategory> categoryRepository;
        private readonly IGenericRepository<ProductBrand> brandRepository;

        public ProductController(IGenericRepository<Product> ProductRepository
            , IMapper mapper
            , IGenericRepository<ProductCategory> categoryRepository
            , IGenericRepository<ProductBrand> brandRepository)
        {
            this.productRepository = ProductRepository;
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
            this.brandRepository = brandRepository;
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResponse<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecificationsParams specsParams)
        {
            var specs = new ProductWithBrandAndCategorySpecifications(specsParams);
            var products = await productRepository.GetAllWithSpecificationAsync(specs);
            #region Getting Count Of Data
            var countSpecs = new ProductWithFiltrationForCountSpecifications(specsParams);
            var count = await productRepository.GetCountAsync(countSpecs);
            #endregion
            var data = mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);
            return Ok(new PaginationResponse<ProductToReturnDto>(specsParams.PageIndex, specsParams.PageSize, count, data));

        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var specs = new ProductWithBrandAndCategorySpecifications(id);
            var product = await productRepository.GetWithSpecificationAsync(specs);
            if (product is null)
                return NotFound(new ApiResponse(404, "Product Not Found"));
            return Ok(mapper.Map<ProductToReturnDto>(product));
        }


        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await brandRepository.GetAllAsync();

            return Ok(brands);

        }
        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
        {
            var categories = await categoryRepository.GetAllAsync();

            return Ok(categories);

        }

    }
}
