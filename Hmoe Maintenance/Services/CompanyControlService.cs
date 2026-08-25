using Hmoe_Maintenance.DataBase;
using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.DTOs.Response.filter;
using Hmoe_Maintenance.Models;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services
{
    public class CompanyControlService : ICompanyControlService
    {
        private readonly AppDBcontext _Context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompanyControlService(AppDBcontext context, UserManager<ApplicationUser> userManager)
        {
            _Context = context;
            _userManager = userManager;
        }

        public async Task<PaginationResponse<Notification>> GetAllNotificationToCompany(string userid,FilternotificationRequest filternotification,int page)
        {
            var notification =  _Context.Notification.Where(e=>e.UserId == userid)
                .OrderByDescending(e=>e.CreatedAt)
                .Take(30);

            if (notification == null)
                return null;


            FilternotificationRespons filternotificationRespons = new();
            if (filternotification.RelatedEntityId != null)
            {
                notification = notification.Where(n => n.RelatedEntityId == filternotification.RelatedEntityId);
                filternotificationRespons.RelatedEntityId = filternotification.RelatedEntityId;
            }
            if (filternotification.msg != null)
            {
                notification = notification.Where(n => n.Message.Contains(filternotification.msg));
                filternotificationRespons.msg = filternotification.msg;
            }
            if (filternotification.IsRead.HasValue)
            {
                notification = notification.Where(n => n.IsRead == filternotification.IsRead.Value);
                filternotificationRespons.Isread = filternotification.IsRead.Value;
            }

            var Result = await PaginationService.PaginateAsync(notification, page, 5);

            return Result;
        }

        public async Task<Notification> GetAllNotificationBycompanyById(int id, string comid)
        {
            var allNotifications = await _Context.Notification.FirstOrDefaultAsync(e => e.UserId == comid && e.Id == id);
            if (allNotifications == null)
            { return null; }

            return allNotifications;

        }


        public async Task<PaginationResponse<PaymentResponse>> GetAllPaymentbyClient(string comid ,FilterclientRequest filter,int page)
        {
            var payment =  _Context.Payment.Where(e=>e.MaintenanceRequest.Company.ApplicationUserId == comid )
                .Include(e => e.MaintenanceRequest).ThenInclude(e=>e.Customer).AsNoTracking().AsQueryable();

            var showPayments = payment.Select(e => new PaymentResponse
            {
                // Payment
                Id = e.Id,
                Amount = e.Amount,
                PaymentMethod = e.PaymentMethod,
                Status = e.Status,
                TransactionId = e.TransactionId,
                SessionId = e.sessionId,
                GatewayName = e.GatewayName,
                CreatedAt = e.CreatedAt,
                PaidAt = e.PaidAt,
                CancelledAt = e.CancelledAt,
                StripePaymentIntentId = e.StripePaymentIntentId,
                StripeSessionId = e.StripeSessionId,

                // Maintenance
                MaintenanceRequestId = e.MaintenanceRequestId,
                RequestNumber = e.MaintenanceRequest.RequestNumber,
                MaintenanceDescription = e.MaintenanceRequest.Description,
                MaintenanceStatus = e.MaintenanceRequest.Status,

                ServiceName = e.MaintenanceRequest.ServiceCategory.Name,

                Governorate = e.MaintenanceRequest.Governorate,
                City = e.MaintenanceRequest.City,
                Street = e.MaintenanceRequest.Street,
                BuildingNumber = e.MaintenanceRequest.BuildingNumber,
                Floor = e.MaintenanceRequest.Floor,

                PreferredDate = e.MaintenanceRequest.PreferredDate,
                PreferredStartTime = e.MaintenanceRequest.PreferredStartTime,
                PreferredEndTime = e.MaintenanceRequest.PreferredEndTime,

                InspectionPrice = e.MaintenanceRequest.InspectionPrice,
                EstimatedPrice = e.MaintenanceRequest.EstimatedPrice,
                AdditionalCostsTotal = e.MaintenanceRequest.AdditionalCostsTotal,
                FinalPrice = e.MaintenanceRequest.FinalPrice,

                // Customer
                CustomerId = e.MaintenanceRequest.CustomerId,
                CustomerFullName = e.MaintenanceRequest.Customer.FullName,
                CustomerEmail = e.MaintenanceRequest.Customer.Email,
                CustomerPhone = e.MaintenanceRequest.Customer.PhoneNumber
            });


            FilterclientResponse filterResponse = new();

            if (filter.ClientName != null)
            {
                showPayments = showPayments
                    .Where(e => e.CustomerFullName.Contains(filter.ClientName));

                filterResponse.ClientName = filter.ClientName;
            }

            if (filter.RequestNumber != null)
            {
                showPayments = showPayments
                    .Where(e => e.RequestNumber == filter.RequestNumber);

                filterResponse.RequestNumber = filter.RequestNumber;
            }

            if (filter.Status.HasValue)
            {
                showPayments = showPayments
                    .Where(e => e.Status == filter.Status.Value);

                filterResponse.Status = filter.Status.Value;
            }

            if (filter.PaymentMethod.HasValue)
            {
                showPayments = showPayments
                    .Where(e => e.PaymentMethod == filter.PaymentMethod.Value);

                filterResponse.PaymentMethod = filter.PaymentMethod.Value;
            }

            var result =await PaginationService.PaginateAsync(showPayments, page, 5);

            return result;
        }

        public async Task<bool> ApprovecompanyRequest(int notifid)
        {
            var notification = await _Context.Notification.FirstOrDefaultAsync(e => e.Id == notifid);

            if (notification == null)
            {
                return false;
            }

            var requestman = await _Context.MaintenanceRequests
                .Include(e => e.Customer)
                .Include(e => e.Company)
                .FirstOrDefaultAsync(e => e.RequestNumber == notification.RelatedEntityId );

            if (requestman == null)
            {
                return false;
            }
            requestman.Status = MaintenanceRequestStatus.CompanyAccepted;
            notification.IsRead = true;

            var notif = new Notification
            {
                UserId = requestman.CustomerId,
                Title = $"Request Accepted from: {requestman.Company.Name}",
                Message = $"Your maintenance request ({requestman.RequestNumber}) has been accepted. A technician will be assigned soon.",
                Type = NotificationType.CompanyAccepted,
                IsRead = false,
                RelatedEntityId = requestman.CompanyId.ToString(),
                CreatedAt = DateTime.UtcNow
            };

            await _Context.Notification.AddAsync(notif);
            await _Context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RejectCompanyRequest(int notifid)
        {
            var notification = await _Context.Notification
                .FirstOrDefaultAsync(e => e.Id == notifid);

            if (notification == null)
            {
                return false;
            }
            notification.IsRead = true;

            var requestman = await _Context.MaintenanceRequests
                .Include(e => e.Customer)
                  .Include(e => e.Company)
                .FirstOrDefaultAsync(e => e.RequestNumber == notification.RelatedEntityId);

            if (requestman == null)
            {
                return false;
            }

            requestman.Status = MaintenanceRequestStatus.Companyrejectedrequest;
            notification.IsRead = true;

            var notif = new Notification
            {
                UserId = requestman.CustomerId,
                Title = $"Maintenance Request Rejected from: {requestman.Company.Name}",
                Message = $"Unfortunately, your maintenance request ({requestman.RequestNumber}) has been rejected by the company.",
                Type = NotificationType.CompanyRejected,
                IsRead = false,
                RelatedEntityId = requestman.RequestNumber,
                CreatedAt = DateTime.UtcNow
            };

            await _Context.Notification.AddAsync(notif);
            await _Context.SaveChangesAsync();

            return true;
        }

        public async Task<CreatepriceRequest> Createprisebycompany(int notifid, CreatepriceRequest createprise)
        {
            var notifation = await _Context.Notification.FirstOrDefaultAsync(e => e.Id == notifid);

            if (notifation == null)
            {
                return null;
            }
            notifation.IsRead = true;

            var requestman = await _Context.MaintenanceRequests
                .Include(e => e.Customer)
                  .Include(e => e.Company)
                .FirstOrDefaultAsync(e => e.RequestNumber == notifation.RelatedEntityId && e.Status == MaintenanceRequestStatus.CompanyAccepted);

            if (requestman == null)
            { return null; }

            requestman.EstimatedPrice = createprise.EstimatedPrice;
            requestman.InspectionPrice = createprise.InspectionPrice;
            requestman.FinalPrice = requestman.EstimatedPrice + requestman.InspectionPrice;

            var notification = new Notification
            {
                UserId = requestman.CustomerId,
                Title = $"Price Estimate Ready from: {requestman.Company.Name}",
                Message = $"Your maintenance request ({requestman.RequestNumber}) has been inspected.\n\n" +
                  $"Inspection Fee: {requestman.InspectionPrice:C}\n" +
                  $"Estimated Repair Cost: {requestman.EstimatedPrice:C}\n" +
                  $"Total Price: {requestman.FinalPrice:C}\n\n" +
                  $"{createprise.Notes}"+
                  $"Please review the estimate and choose to accept or reject it.",
                Type = NotificationType.WaitingCustomerOfferResponseprice,
                IsRead = false,
                RelatedEntityId = requestman.RequestNumber,
                CreatedAt = DateTime.UtcNow
            };

            await _Context.Notification.AddAsync(notification);
            await _Context.SaveChangesAsync();

            return createprise;
        }

        public async Task<bool> AssignedTechnicianRequest(int id, int Tecid)
        {
            var notifation = await _Context.Notification.FirstOrDefaultAsync(e => e.Id == id);

            if (notifation == null)
            {
                return false;
            }
            notifation.IsRead = true;

            var requestman = await _Context.MaintenanceRequests
                .Include(e => e.Customer)
                .Include(e=>e.Company)
                .Include(e => e.ServiceCategory)
                .Include(e => e.AssignedTechnician)
                .FirstOrDefaultAsync(e => e.RequestNumber == notifation.RelatedEntityId && e.Status== MaintenanceRequestStatus.clientApproveprice);

            if (requestman == null)
            { return false; }

            var Techniciancopy = await _Context.TechnicianProfileCopies.FirstOrDefaultAsync(e => e.Id == Tecid && e.IsAvailable && e.IsActive);
            if (Techniciancopy == null)
            { return false; }

            var Technician =await _Context.TechnicianProfiles.FirstOrDefaultAsync(e => e.UserId == Techniciancopy.UserId);

            requestman.technicianProfileCopyId = Tecid;
            requestman.AssignedTechnicianId = Technician.Id;
            requestman.Status = MaintenanceRequestStatus.TechnicianAssigned;
            Techniciancopy.IsActive = false;
            Technician.IsActive = false;

            var noti = new Notification
            {
                UserId = Techniciancopy.UserId,
                Title = $"New Maintenance Request Assigned from: {requestman.Company.Name}",
                Message =
                $"You have been assigned a new maintenance request.\n\n" +
                $"Request Number: {requestman.RequestNumber}\n" +
                $"Customer: {requestman.Customer.FullName}\n" +
                $"Address: {requestman.Address}\n" +
                $"Service: {requestman.ServiceCategory.Name}\n" +  
                $"phone: {requestman.Phone}\n" +
                $"Preferred Date: {requestman.PreferredDate:dd/MM/yyyy}\n\n" +
                $"Please review the request and contact the customer if needed.",
                Type = NotificationType.TechnicianAssigned,
                IsRead = false,
                RelatedEntityId = requestman.RequestNumber,
                CreatedAt = DateTime.UtcNow
            };

            await _Context.Notification.AddAsync(noti);
            await _Context.SaveChangesAsync();

            return true;
        }
    }
}
