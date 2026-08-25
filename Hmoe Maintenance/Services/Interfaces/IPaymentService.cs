using Hmoe_Maintenance.Models;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<Payment> CreatePayment(string userid);
        Task<Payment> HandlePaymentSuccess(string orderId,Session session);

        Task<Payment> HandlePaymentCancel(string orderId);
    }
}
