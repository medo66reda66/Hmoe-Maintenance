using Hmoe_Maintenance.DataBase;
using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.Models;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace Hmoe_Maintenance.Services
{
    public class MaintenanceRequestService : IMaintenanceRequestService
    {
        private readonly AppDBcontext _Context;

        public MaintenanceRequestService(AppDBcontext context)
        {
            _Context = context;
        }

        public async Task<List<Notification>> GetAllNotificationToCompany(string clientid)
        {
            var notifications = await _Context.Notification.Where(e => e.UserId == clientid && e.IsRead == false)
                .OrderByDescending(e => e.CreatedAt)
                .Take(30).ToListAsync();

            return notifications;
        }
        public async Task<CreateMaintenanceRequest> createMaintenance(CreateMaintenanceRequest createMaintenanceRequest, string userid)
        {
            
            var adress = await _Context.Addresses.FirstOrDefaultAsync(e => e.ApplicationUserId == userid);
            var request = new MaintenanceRequest
            {
                RequestNumber = Guid.NewGuid().ToString(),
                CustomerId = userid,
                CompanyId = createMaintenanceRequest.CompanyId,
                ServiceCategoryId = createMaintenanceRequest.ServiceCategoryId,
                AddressId = adress.Id,
                Governorate = createMaintenanceRequest.Governorate,
                City = createMaintenanceRequest.City,
                Street = createMaintenanceRequest.Street,
                BuildingNumber = createMaintenanceRequest.BuildingNumber,
                Floor = createMaintenanceRequest.Floor,
                Phone = createMaintenanceRequest.Phone,
                Description = createMaintenanceRequest.Description,
                PreferredDate = createMaintenanceRequest.PreferredDate,
                Status = MaintenanceRequestStatus.PendingCompanyApproval,
                CreatedAt = DateTime.UtcNow,
            };

            await _Context.MaintenanceRequests.AddAsync(request);
            await _Context.SaveChangesAsync();

            if (createMaintenanceRequest.ImageUrlS != null)
            {
                foreach (var image in createMaintenanceRequest.ImageUrlS)
                {
                    var FilenameFFFrontImageUrl = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var filePathFrontImageUrl = Path.Combine("wwwroot", "MaintenanceRequestImage", FilenameFFFrontImageUrl);
                    using (var stream = new FileStream(filePathFrontImageUrl, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    var maintenanceImage = new MaintenanceRequestImage
                    {
                        MaintenanceRequestId = request.Id,
                        UploadedByUserId = userid,
                        CreatedAt = DateTime.UtcNow,
                        IsAfterWork = false,
                        IsBeforeWork = true,
                        ImageUrl = filePathFrontImageUrl
                    };
                    await _Context.AddAsync(maintenanceImage);
                }
            }

            var company = await _Context.Companies
               .FirstOrDefaultAsync(c => c.Id == createMaintenanceRequest.CompanyId);

            var notification = new Notification
            {
                UserId = company.ApplicationUserId,
                Title = "New Maintenance Request",
                Message = $"A new maintenance request ({request.RequestNumber}) has been submitted and is waiting for your review.",
                Type = NotificationType.PendingCompanyApproval,
                IsRead = false,
                RelatedEntityId = request.RequestNumber,
                CreatedAt = DateTime.UtcNow
            };
            await _Context.Notification.AddAsync(notification);
            await _Context.SaveChangesAsync();

            return createMaintenanceRequest;
        }

        public async Task<bool> Approveprice(int notificationId)
        {
            var notification = await _Context.Notification
               .FirstOrDefaultAsync(n => n.Id == notificationId);

            if (notification == null)
                return false;
            notification.IsRead = true;

            var request =await _Context.MaintenanceRequests.Include(e=>e.Company)
                .FirstOrDefaultAsync(e => e.RequestNumber == notification.RelatedEntityId);

            request.Status = MaintenanceRequestStatus.clientApproveprice;

            var companyNotification = new Notification
            {
                UserId = request.Company.ApplicationUserId,
                Title = "Price Offer Accepted",
                Message = "The customer has accepted your price offer. You can now assign a technician to begin the maintenance request.",
                Type = NotificationType.PriceOfferAccepted,
                RelatedEntityId = request.RequestNumber
            };

            await _Context.Notification.AddAsync(companyNotification);
            await _Context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> RejectPrice(int notificationId)
        {
            var notification = await _Context.Notification
                .FirstOrDefaultAsync(n => n.Id == notificationId);

            if (notification == null)
                return false;

            notification.IsRead = true;

            var request = await _Context.MaintenanceRequests
                .Include(e => e.Company)
                .FirstOrDefaultAsync(e => e.RequestNumber == notification.RelatedEntityId);

            if (request == null)
                return false;

            request.Status = MaintenanceRequestStatus.clientrejectedprice;

            var companyNotification = new Notification
            {
                UserId = request.Company.ApplicationUserId,
                Title = "Price Offer Rejected",
                Message = "The customer has rejected your price offer. You can review the offer and send a new one if needed.",
                Type = NotificationType.PriceOfferRejected,
                RelatedEntityId = request.RequestNumber,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _Context.Notification.AddAsync(companyNotification);
            await _Context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ApproveAdditionalCost(int notificationId)
        {
            var notification = await _Context.Notification
                .FirstOrDefaultAsync(n => n.Id == notificationId);

            if (notification == null)
                return false;
            notification.IsRead = true;

            var additionalCost = await _Context.AdditionalCostRequests
                .Include(a => a.MaintenanceRequest)
                    .ThenInclude(r => r.AssignedTechnician)
                .FirstOrDefaultAsync(a =>
                    a.Id == int.Parse(notification.RelatedEntityId!));

            if (additionalCost == null)
                return false;

            additionalCost.Status = AdditionalCostStatus.Approved;
            additionalCost.RespondedAt = DateTime.UtcNow;

            additionalCost.MaintenanceRequest.Status =
                MaintenanceRequestStatus.WorkInProgress;

            var noti = new Notification
            {
                UserId = additionalCost.MaintenanceRequest.AssignedTechnician.UserId,
                Title = "Additional Cost Approved",
                Message = $"The customer has approved the additional cost for maintenance request ({additionalCost.MaintenanceRequest.RequestNumber}). You may continue the repair work.",
                Type = NotificationType.AdditionalCostApproved,
                IsRead = false,
                RelatedEntityId = additionalCost.MaintenanceRequest.RequestNumber,
                CreatedAt = DateTime.UtcNow
            };

            await _Context.Notification.AddAsync(noti);
            await _Context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> RejectAdditionalCost(int notificationId, string? note)
        {
            var notification = await _Context.Notification
                .FirstOrDefaultAsync(n => n.Id == notificationId);

            if (notification == null)
                return false;

            notification.IsRead = true;

            var additionalCost = await _Context.AdditionalCostRequests
                .Include(a => a.MaintenanceRequest)
                    .ThenInclude(r => r.AssignedTechnician)
                .FirstOrDefaultAsync(a =>
                    a.Id == int.Parse(notification.RelatedEntityId!));

            if (additionalCost == null)
                return false;

            additionalCost.Status = AdditionalCostStatus.Rejected;
            additionalCost.CustomerResponseNote = note;
            additionalCost.RespondedAt = DateTime.UtcNow;
            
            var noti = new Notification
            {
                UserId = additionalCost.MaintenanceRequest.AssignedTechnician.UserId,
                Title = "Additional Cost Rejected",
                Message = $"The customer has rejected the additional cost request for maintenance request ({additionalCost.MaintenanceRequest.RequestNumber}). Please review the request or contact the customer.",
                Type = NotificationType.AdditionalCostRejected,
                IsRead = false,
                RelatedEntityId = additionalCost.MaintenanceRequest.RequestNumber,
                CreatedAt = DateTime.UtcNow
            };

            await _Context.Notification.AddAsync(noti);
            await _Context.SaveChangesAsync();

            return true;
        }



       //pay




    }
}
