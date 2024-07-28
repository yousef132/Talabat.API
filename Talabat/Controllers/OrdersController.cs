using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    [Authorize]

    public class OrderController : BaseController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        [HttpPost]

        public async Task<ActionResult<OrderToReturnDto>> CreateOrderAsync(CreateOrderDto orderDto)
        {
            var address = mapper.Map<Address>(orderDto.Address);

            var order = await orderService.CreateOrderAsync(orderDto.CartId, address, orderDto.BuyerEmail, orderDto.DeliveryMethodId);

            if (order is null)
                return BadRequest(new ApiResponse(400));


            var mappedOrder = mapper.Map<OrderToReturnDto>(order);
            return Ok(mappedOrder);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetUserOrders(string email)
        {
            var orders = await orderService.GetOrdersOfUserAsync(email);
            var mappedOrders = mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);

            return Ok(mappedOrders);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id, string email)
        {
            var order = await orderService.GetOrderByIdForUser(email, id);
            if (order is null)
                return NotFound(new ApiResponse(404, "Order Not Fount"));

            var mappedOrder = mapper.Map<OrderToReturnDto>(order);

            return Ok(mappedOrder);
        }


        [HttpGet("deliverymethod")]

        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethods = await orderService.GetDeliveryMethodsAsync();

            return Ok(deliveryMethods);
        }
    }
}
