using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Cart;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    public class PaymentController : BaseController
    {
        private readonly IPaymentService paymentService;
        private readonly ILogger<PaymentController> logger;

        // This is your Stripe CLI webhook secret for testing your endpoint locally.
        private const string endpointSecret = "whsec_f856987b161b307ced0ba2a58466fafd555146c41fbe42c162bb8be07f7a91f3";

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            this.paymentService = paymentService;
            this.logger = logger;
        }
        [HttpGet("{cartId}")]
        [ProducesResponseType(typeof(CustomerCart), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult<CustomerCart>> CreateOrUpdatePaymentIntent(string cartId)
        {
            var cart = await paymentService.CreateOrUpdatePaymentIntent(cartId);
            if (cart == null)
                return BadRequest(new ApiResponse(400, "An Error With Your Cart"));

            return Ok(cart);
        }


        [HttpPost]
        [Route("webhook")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], endpointSecret);

            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            Order? order;
            // Handle the event
            if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
            {
                order = await paymentService.UpdateOrderStatus(paymentIntent.Id, false);
                logger.LogError("Order is failed {0}", order?.PaymentIntentId);
                logger.LogInformation("Unhandled even type: {0}", stripeEvent.Type);

            }
            else
            {
                order = await paymentService.UpdateOrderStatus(paymentIntent.Id, true);
                logger.LogInformation("Order is succeeded {0}", order?.PaymentIntentId);

            }
            return Ok();

        }
    }
}
