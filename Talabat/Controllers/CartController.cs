using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Cart;
using Talabat.Core.Repositories.Contract;

namespace Talabat.APIs.Controllers
{

    public class CartController : BaseController
    {
        private readonly ICartRepository cartRepository;
        private readonly IMapper mapper;

        public CartController(ICartRepository cartRepository, IMapper mapper)
        {
            this.cartRepository = cartRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<CustomerCart>> GetCartAsync(string id)
        {
            var cart = await cartRepository.GetCartAsync(id);
            if (cart == null)
                return NotFound(new ApiResponse(404));

            return Ok(cart);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerCart>> UpdateCartAsync(CustomerCartDto cart)
        {

            var mappedCart = mapper.Map<CustomerCart>(cart);
            var createdOrUpdatedCart = await cartRepository.UpdateCartAsync(mappedCart);

            if (createdOrUpdatedCart == null)
                return BadRequest(new ApiResponse(400));

            return Ok(createdOrUpdatedCart);

        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteCart(string cartId)
        {
            return await cartRepository.DeleteCartAsync(cartId);

        }
    }
}
