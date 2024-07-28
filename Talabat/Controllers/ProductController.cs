using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities.Product_Aggregate;
using Talabat.Core.Services.Contract;
using Talabat.Core.Spepcifications.ProductsSpecifications;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductService productService;
        private readonly IMapper mapper;

        public ProductsController(IProductService productService
            , IMapper mapper
            )
        {
            this.productService = productService;
            this.mapper = mapper;
        }


        [HttpGet]
        [Authorize]
        [Cache(600)]
        public async Task<ActionResult<PaginationResponse<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecificationsParams specsParams)
        {
            var products = await productService.GetProductsAsync(specsParams);

            #region Getting Count Of Data
            var count = await productService.GetCountAsync(specsParams);
            #endregion

            var data = mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);
            return Ok(new PaginationResponse<ProductToReturnDto>(specsParams.PageIndex, specsParams.PageSize, count, data));
        }


        [HttpGet]
        [Route("{id}")]
        [Cache(600)]

        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await productService.GetProductByIdAsync(id);
            if (product is null)
                return NotFound(new ApiResponse(404, "Product Not Found"));
            return Ok(mapper.Map<ProductToReturnDto>(product));
        }


        [HttpGet("brands")]
        [Cache(600)]

        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await productService.GetBrandsAsync();
            return Ok(brands);

        }


        [HttpGet("categories")]
        [Cache(600)]

        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
        {
            var categories = await productService.GetBrandsAsync();

            return Ok(categories);

        }

    }
}
