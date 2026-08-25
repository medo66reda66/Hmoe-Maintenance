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

        public async Task<PaginationResponse<Notification>> GetAllNotificationToClient(string clientid,FilternotificationRequest filternotification,int page)
        {
            var notification =  _Context.Notification.Where(e => e.UserId == clientid && e.IsRead == false)
                .OrderByDescending(e => e.CreatedAt)
                .Take(30);

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
        public async Task<Notification> GetNotificationByclientById(int id, string clienid)
        {
            var allNotifications = await _Context.Notification.FirstOrDefaultAsync(e => e.UserId == clienid && e.Id == id);
            if (allNotifications == null)
            { return null; }

            return allNotifications;

        }

        public async Task<PaginationResponse<MaintenanceRequestResponse>> GetAllMaintenanceRequestByClient(string clientid,FilterMaintenanceRequest filter,int page)
        {
            var maintenanceRequests = _Context.MaintenanceRequests
               .Where(e => e.CustomerId == clientid)
               .OrderByDescending(e => e.CreatedAt)
               .Take(30)
               .Select(e => new MaintenanceRequestResponse
               {
                   Id = e.Id,
                   RequestNumber = e.RequestNumber,

                   CustomerId = e.CustomerId,

                   CompanyId = e.CompanyId,
                   CompanyName = e.Company.Name,
                   CompanyEmail = e.Company.Email,

                   ServiceCategoryId = e.ServiceCategoryId,
                   ServiceName = e.ServiceCategory.Name,

                   Governorate = e.Governorate,
                   City = e.City,
                   Street = e.Street,
                   BuildingNumber = e.BuildingNumber,
                   Floor = e.Floor,
                   Phone = e.Phone,

                   Description = e.Description,

                   PreferredDate = e.PreferredDate,
                   PreferredStartTime = e.PreferredStartTime,
                   PreferredEndTime = e.PreferredEndTime,

                   TechnicianId = e.AssignedTechnicianId,
                   TechnicianFullName = e.AssignedTechnician != null
                       ? e.AssignedTechnician.Fullname
                       : null,
                   TechnicianEmail = e.AssignedTechnician != null
                       ? e.AssignedTechnician.Email
                       : null,
                   TechnicianPhone = e.AssignedTechnician != null
                       ? e.AssignedTechnician.PhoneNumper
                       : null,

                   InspectionPrice = e.InspectionPrice,
                   EstimatedPrice = e.EstimatedPrice,
                   AdditionalCostsTotal = e.AdditionalCostsTotal,
                   FinalPrice = e.FinalPrice,

                   Status = e.Status,

                   PaymentApproved = e.PaymentApproved,
                   PaymentRejected = e.PaymentRejected,

                   TechnicianReport = e.TechnicianReport,

                   CreatedAt = e.CreatedAt,
                   CompletedAt = e.CompletedAt
               })
               .AsQueryable();

             FilterMaintenanceResponse maintenanceResponse = new FilterMaintenanceResponse();
            if (filter.RequestNumber != null)
            {
                maintenanceRequests = maintenanceRequests
                    .Where(e => e.RequestNumber.Contains(filter.RequestNumber));
                maintenanceResponse.RequestNumber = filter.RequestNumber;
            }
            if (filter.CompanyName != null)
            {
                maintenanceRequests = maintenanceRequests
                    .Where(e => e.CompanyName.Contains(filter.CompanyName));
                maintenanceResponse.CompanyName = filter.CompanyName;
            }
            if (filter.Governorate != null)
            {
                maintenanceRequests = maintenanceRequests
                    .Where(e => e.Governorate.Contains(filter.Governorate));
                maintenanceResponse.Governorate = filter.Governorate;
            }
            if (filter.City != null)
            {
                maintenanceRequests = maintenanceRequests
                    .Where(e => e.City.Contains(filter.City));
                maintenanceResponse.City = filter.City;
            }
            var result =await PaginationService.PaginateAsync(maintenanceRequests, page, 5);

            return result;

        }
        public async Task<List<Payment>> GetallPaymentByMaintenanceRequestId(string clientid)
        {
            var payment = await _Context.Payment
                .Where(p => p.MaintenanceRequest.CustomerId == clientid)
                  .OrderByDescending(e => e.CreatedAt)
                .Take(30)
                .ToListAsync();

            if (payment == null)
            {
                return null;
            }
            return payment;
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
                PaymentApproved = false,
                PaymentRejected = false,
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

            var customer = await _Context.ApplicationUsers
                .FirstOrDefaultAsync(c => c.Id == userid);

            var notification = new Notification
            {
                UserId = company.ApplicationUserId,
                Title = $"New Maintenance Request form:{customer.FullName}",
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

            var request =await _Context.MaintenanceRequests.Include(e=>e.Company).Include(e=>e.Customer)
                .FirstOrDefaultAsync(e => e.RequestNumber == notification.RelatedEntityId);

            request.Status = MaintenanceRequestStatus.clientApproveprice;

            var companyNotification = new Notification
            {
                UserId = request.Company.ApplicationUserId,
                Title = $"Price Offer Accepted from:{request.Customer.FullName}",
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
                .Include(e => e.Customer)
                .FirstOrDefaultAsync(e => e.RequestNumber == notification.RelatedEntityId);

            if (request == null)
                return false;

            request.Status = MaintenanceRequestStatus.clientrejectedprice;

            var companyNotification = new Notification
            {
                UserId = request.Company.ApplicationUserId,
                Title = $"Price Offer Rejected from:{request.Customer.FullName}",
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
                .Include(a => a.MaintenanceRequest.Customer.FullName)
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
                Title = $"Additional Cost Approved from:{additionalCost.MaintenanceRequest.Customer.FullName}",
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
                .Include(a => a.MaintenanceRequest.Customer.FullName)
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
                Title = $"Additional Cost Rejected from:{additionalCost.MaintenanceRequest.Customer.FullName}",
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

        public async Task<Models.Review> Review(int maintenanceRequestId, string userId, int rating, string comment)
        {
            var maintenanceRequest = await _Context.MaintenanceRequests
                .FirstOrDefaultAsync(m => m.Id == maintenanceRequestId && m.CustomerId == userId);

            if (maintenanceRequest == null || maintenanceRequest.Status != MaintenanceRequestStatus.Completed)
            {
                throw new InvalidOperationException("Maintenance request not found or not completed.");
            }
            var review = new Models.Review
            {
                MaintenanceRequestId = maintenanceRequestId,
                CustomerId = userId,
                TechnicianProfileId = maintenanceRequest.technicianProfileCopyId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.UtcNow
            };
            await _Context.Reviews.AddAsync(review);
            await _Context.SaveChangesAsync();
            return review;
        }



        //pay




    }
}
