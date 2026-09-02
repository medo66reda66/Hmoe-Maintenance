using Hmoe_Maintenance.DataBase;
using Hmoe_Maintenance.Models;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using Stripe.Climate;

namespace Hmoe_Maintenance.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDBcontext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public PaymentService(AppDBcontext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<Payment> CreatePayment(string userid)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userid);

                if (user == null)
                {
                    throw new System.Exception("User not found.");
                }
                var maintenanceRequest = await _context.MaintenanceRequests
                    .FirstOrDefaultAsync(e=>e.CustomerId == user.Id && e.PaymentApproved == false && e.Status==MaintenanceRequestStatus.WorkCompleted);

                    if (maintenanceRequest == null)
                    {
                        throw new System.Exception("Maintenance request not found.");
                    }

                var tatalCost = (maintenanceRequest.FinalPrice + maintenanceRequest.AdditionalCostsTotal);

                    var payment = new Payment
                    {
                        MaintenanceRequestId = maintenanceRequest.Id,
                        Amount = tatalCost,
                        PaymentMethod = PaymentMethod.Card,
                        Status = PaymentStatus.Pending,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _context.Payment.AddAsync(payment);
                    await _context.SaveChangesAsync();

                    return payment;
                
            }
            catch (System.Exception ex)
            {
                throw new System.Exception($"An error occurred while creating the payment: {ex.Message}");
            }
        }
        public async Task<Payment> HandlePaymentSuccess(string paymentId ,Session session)
        {
            try
            {
                var payment = await _context.Payment.Include(e=>e.MaintenanceRequest).FirstOrDefaultAsync(p => p.Id.ToString() == paymentId 
                && p.sessionId == session.Id);
                if (payment == null)
                {
                    throw new System.Exception("Payment not found.");
                }
                payment.Status = PaymentStatus.Paid;
                payment.PaidAt = DateTime.UtcNow;
                payment.GatewayName = "Stripe";
                payment.MaintenanceRequest.PaymentApproved = true;

                var tech = await _context.TechnicianProfileCopies
                          .FirstOrDefaultAsync(e => e.Id == payment.MaintenanceRequest.technicianProfileCopyId);
                if (tech == null)
                {
                    return null;
                }
                tech.TotalAmount += (payment.Amount - (payment.Amount * (tech.RevenueShare / 100)));

                await _context.SaveChangesAsync();
                return payment;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception($"An error occurred while handling the payment success: {ex.Message}");
            }
        }
        public async Task<Payment> HandlePaymentCancel(string paymentId)
        {
            try
            {
                var payment = await _context.Payment.FirstOrDefaultAsync(p => p.Id.ToString() == paymentId);
                if (payment == null)
                {
                    throw new System.Exception("Payment not found.");
                }
                payment.Status = PaymentStatus.Cancelled;
                payment.CancelledAt = DateTime.UtcNow;
                payment.MaintenanceRequest.PaymentRejected = true;
                await _context.SaveChangesAsync();
                return payment;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception($"An error occurred while handling the payment cancellation: {ex.Message}");
            }
        }
    }
}