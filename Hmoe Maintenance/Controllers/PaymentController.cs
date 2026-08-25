using Hmoe_Maintenance.DataBase;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe.Climate;
using System.Security.Claims;

namespace Hmoe_Maintenance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly AppDBcontext _appD;
        public PaymentController(IPaymentService paymentService, AppDBcontext appD)
        {
            _paymentService = paymentService;
            _appD = appD;
        }
        [HttpPost("Payment")]
        public async Task<IActionResult> Payment()
        {
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);

            var result = await _paymentService.CreatePayment(userId);
            if (result == null)
            {
                return BadRequest("Failed to create payment.");
            }
            var options = new SessionCreateOptions
            {   
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/api/Payment/PaymentSuccess/{result.Id}/session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/api/Payment/PaymentCancel/{result.Id}",
            };

            options.LineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "egp",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = result.MaintenanceRequest.RequestNumber,
                        Description = "Payment for maintenance request",
                    },
                    UnitAmount = (long)result.Amount * 100,
                },
                Quantity = 1,
            });

            var service = new SessionService();
            var session = service.Create(options);

            result.sessionId = session.Id;
            await _appD.SaveChangesAsync();

            return Ok(session.Url);
        }
        [AllowAnonymous]
        [HttpGet("PaymentSuccess/{paymentId}/{session_id}")]
        public async Task<IActionResult> PaymentSuccess(string paymentId,string session_id)
        {
            var stripeService = new SessionService();
            var session = await stripeService.GetAsync(session_id);
            var result = await _paymentService.HandlePaymentSuccess(paymentId,session);
            if (result == null)
            {
                return BadRequest("Failed to process payment success.");
            }
            return Ok(result);

        }
        [AllowAnonymous]
        [HttpGet("PaymentCancel/{paymentId}")]
        public async Task<IActionResult> PaymentCancel(string paymentId)
        {
            var result = await _paymentService.HandlePaymentCancel(paymentId);
            if (result == null)
            {
                return BadRequest("Failed to process payment cancellation.");
            }
            return Ok(result);
        }
    }
}
