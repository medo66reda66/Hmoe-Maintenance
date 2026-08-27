using Hmoe_Maintenance.DataBase;
using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.DTOs.Response.filter;
using Hmoe_Maintenance.Models;
using Hmoe_Maintenance.Services.Interfaces;
using Hmoe_Maintenance.SignalRWebAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services
{
    public class TechnicianControlService : ITechnicianControlService
    {
        private readonly AppDBcontext _Context;
        private readonly INotificationService _notificationService;

        public TechnicianControlService(AppDBcontext context, INotificationService notificationService)
        {
            _Context = context;
            _notificationService = notificationService;
        }

        public async Task<PaginationResponse<Notification>> GetAllNotificationByTech(string techid,FilternotificationRequest filternotification,int page)
        {
            var notification = _Context.Notification.Where(e => e.UserId == techid)
                  .OrderByDescending(e => e.CreatedAt)
                .Take(30);

            if (notification == null)
            { return null; }

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
        public async Task<Notification> GetAllNotificationByTechById(int id, string techid)
        {
            var allNotification = await _Context.Notification.FirstOrDefaultAsync(e => e.UserId == techid && e.Id == id);
            if (allNotification == null)
            { return null; }

            return allNotification;

        }
        public async Task<bool> CreateselectTime(int id, TimeSpan Time)
        {
            var notifation = await _Context.Notification.FirstOrDefaultAsync(e => e.Id == id);

            if (notifation == null)
            {
                return false;
            }
            notifation.IsRead = true;

            var requestman = await _Context.MaintenanceRequests
                .Include(e => e.Customer)
                .Include(e => e.AssignedTechnician)
                .FirstOrDefaultAsync(e => e.RequestNumber == notifation.RelatedEntityId);

            if (requestman == null)
            { return false; }

            requestman.PreferredStartTime = Time;

            var noti = new Notification
            {
                UserId = requestman.CustomerId,
                Title = $"Technician Assigned from: {requestman.AssignedTechnician.Fullname}",
                Message = $"A technician has been assigned to your maintenance request ({requestman.RequestNumber}). They will contact you soon." +
                $"time:{requestman.PreferredStartTime}",
                Type = NotificationType.TechnicianAssigned,
                IsRead = false,
                RelatedEntityId = requestman.RequestNumber,
                CreatedAt = DateTime.UtcNow
            };
            await _Context.Notification.AddAsync(noti);
            await _Context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> UpdateSelectTime(int notificationId, TimeSpan? time)
        {
            var notification = await _Context.Notification
                .FirstOrDefaultAsync(e => e.Id == notificationId);

            if (notification == null)
                return false;

            notification.IsRead = true;

            var request = await _Context.MaintenanceRequests
                .Include(e => e.AssignedTechnician)
                .FirstOrDefaultAsync(e =>
                    e.RequestNumber == notification.RelatedEntityId);

            if (request == null)
                return false;


            request.PreferredStartTime = time ?? request.PreferredStartTime;

            var newNotification = new Notification
            {
                UserId = request.CustomerId,
                Title = $"Appointment Time Updated from: {request.AssignedTechnician.Fullname}",
                Message = $"Your maintenance appointment time has been updated to {time:hh\\:mm}.",
                Type = NotificationType.TechnicianAssigned,
                IsRead = false,
                RelatedEntityId = request.RequestNumber,
                CreatedAt = DateTime.UtcNow
            };
            await _Context.Notification.AddAsync(newNotification);
            await _Context.SaveChangesAsync();
            await _notificationService.SendToUserAsync(notification);
            return true;
        }

        public async Task<bool> TechnicianOnTheWay(int id)
        {
            var notifation = await _Context.Notification.FirstOrDefaultAsync(e => e.Id == id);

            if (notifation == null)
            {
                return false;
            }
            notifation.IsRead = true;
            var requestman = await _Context.MaintenanceRequests
                .Include(e => e.Customer)
                .Include(e => e.AssignedTechnician)
                .FirstOrDefaultAsync(e => e.RequestNumber == notifation.RelatedEntityId);

            if (requestman == null)
            { return false; }

            requestman.Status = MaintenanceRequestStatus.TechnicianOnTheWay;

            var noti = new Notification
            {
                UserId = requestman.CustomerId,
                Title = $"Technician On The Way from: {requestman.AssignedTechnician.Fullname}",
                Message = $"Your technician is on the way to your location for maintenance request ({requestman.RequestNumber}).",
                Type = NotificationType.TechnicianOnTheWay,
                IsRead = false,
                RelatedEntityId = requestman.RequestNumber,
                CreatedAt = DateTime.UtcNow
            };

            await _Context.Notification.AddAsync(noti);
            await _Context.SaveChangesAsync();
            await _notificationService.SendToUserAsync(noti);
            return true;

        }

        public async Task<bool> TechnicianArrived(int id)
        {
            var notifation = await _Context.Notification.FirstOrDefaultAsync(e => e.Id == id);

            if (notifation == null)
            {
                return false;
            }
            notifation.IsRead = true;

            var requestman = await _Context.MaintenanceRequests
                .Include(e => e.Customer)
                .Include(e => e.AssignedTechnician)
                .FirstOrDefaultAsync(e => e.RequestNumber == notifation.RelatedEntityId);

            if (requestman == null)
            { return false; }

            requestman.Status = MaintenanceRequestStatus.TechnicianArrived;


            var noti = new Notification
            {
                UserId = requestman.CustomerId,
                Title = $"Technician Arrived from: {requestman.AssignedTechnician.Fullname}",
                Message = $"Your technician has arrived at your location for maintenance request ({requestman.RequestNumber}).",
                Type = NotificationType.TechnicianArrive,
                IsRead = false,
                RelatedEntityId = requestman.RequestNumber,
                CreatedAt = DateTime.UtcNow
            };

            await _Context.Notification.AddAsync(noti);
            await _Context.SaveChangesAsync();
            await _notificationService.SendToUserAsync(noti);
            return true;

        }

        public async Task<bool> WorkStarted(int id, string Tecid)
        {
            var notifation = await _Context.Notification.FirstOrDefaultAsync(e => e.Id == id);

            if (notifation == null)
            {
                return false;
            }
            var requestman = await _Context.MaintenanceRequests
                .Include(e => e.Customer)
                .Include(e => e.AssignedTechnician)
                .FirstOrDefaultAsync(e => e.RequestNumber == notifation.RelatedEntityId);

            if (requestman == null)
            { return false; }

            requestman.Status = MaintenanceRequestStatus.WorkInProgress;

            var Technician = await _Context.TechnicianProfileCopies.FirstOrDefaultAsync(e => e.UserId == Tecid);

            var noti = new Notification
            {
                UserId = requestman.CustomerId,
                Title = $"Work Started from: {requestman.AssignedTechnician.Fullname}",
                Message = $"The technician has started working on your maintenance request ({requestman.RequestNumber}).",
                Type = NotificationType.WorkInProgress,
                IsRead = false,
                RelatedEntityId = requestman.RequestNumber,
                CreatedAt = DateTime.UtcNow
            };

            await _Context.Notification.AddAsync(noti);
            await _Context.SaveChangesAsync();
            return true;

        }

        public async Task<CreateadditionalcostRequest> AdditionalCost(int id, CreateadditionalcostRequest createadditionalcost)
        {
            var notifation = await _Context.Notification.FirstOrDefaultAsync(e => e.Id == id);

            if (notifation == null)
            {
                return null;
            }
            notifation.IsRead = true;

            var requestman = await _Context.MaintenanceRequests
                .Include(e => e.Customer)
                .Include(e => e.AssignedTechnician)
                .FirstOrDefaultAsync(e => e.RequestNumber == notifation.RelatedEntityId);

            if (requestman == null)
            { return null; }

            var additionalCost = new AdditionalCostRequest
            {
                MaintenanceRequestId = requestman.Id,
                TechnicianProfileId = requestman.AssignedTechnician.Id!,
                LaborCost = createadditionalcost.LaborCost,
                PartsCost = (decimal)createadditionalcost.PartsCost,
                TotalAmount = (decimal)(createadditionalcost.LaborCost + createadditionalcost.PartsCost),
                Reason = createadditionalcost.Reason ?? null,
                Status = AdditionalCostStatus.Pending,
                CreatedAt = DateTime.UtcNow,
            };
            await _Context.AdditionalCostRequests.AddAsync(additionalCost);
            await _Context.SaveChangesAsync();

            requestman.AdditionalCostsTotal = additionalCost.TotalAmount;

            if (createadditionalcost.ImageUrlS is not null)
            {
                foreach (var img in createadditionalcost.ImageUrlS)
                {
                    var FilenameadditionalcostImageUrl = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                    var filePathadditionalcostImageUrl = Path.Combine("wwwroot", "eadditionalcostimgs", FilenameadditionalcostImageUrl);
                    using (var stream = new FileStream(filePathadditionalcostImageUrl, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }
                    var additionalImags = new AdditionalCostImage
                    {
                        AdditionalCostRequestId = additionalCost.Id,
                        ImageUrl = FilenameadditionalcostImageUrl,
                        CreatedAt = DateTime.UtcNow,
                    };
                    await _Context.AdditionalCostImages.AddAsync(additionalImags);
                }
            }
            requestman.Status = MaintenanceRequestStatus.WaitingForCustomerApprovaladditioncost;

            var notification = new Notification
            {
                UserId = requestman.CustomerId,
                Title = $"Additional Cost Approval Required from: {requestman.AssignedTechnician.Fullname}",
                Message = $"The technician has requested additional work for your maintenance request ({requestman.RequestNumber}).\n\n" +
               $"Labor Cost: {additionalCost.LaborCost}\n" +
               $"Parts Cost: {additionalCost.PartsCost}\n" +
               $"Additional Cost: {additionalCost.LaborCost + additionalCost.PartsCost}\n" +
               $"New Total Price: {additionalCost.TotalAmount}\n\n" +
               $"Reason: {additionalCost.Reason}\n\n" +
               $"Please review the request and choose whether to approve or reject the additional cost.",
                Type = NotificationType.AdditionalCostRequested,
                IsRead = false,
                RelatedEntityId = additionalCost.Id.ToString(),
                CreatedAt = DateTime.UtcNow
            };

            await _Context.Notification.AddAsync(notification);
            await _Context.SaveChangesAsync();
            await _notificationService.SendToUserAsync(notification);

            return createadditionalcost;
        }

        public async Task<UpdateadditionalcostRequest> Updateadditionalcost(int id, UpdateadditionalcostRequest updateadditionalcost)
        {
            var additiancost = await _Context.AdditionalCostRequests.Include(e => e.MaintenanceRequest).FirstOrDefaultAsync(e => e.Id == id);
            if (additiancost == null)
            {
                return null;
            }

            additiancost.PartsCost = (decimal)updateadditionalcost.PartsCost;
            additiancost.LaborCost = (decimal)updateadditionalcost.LaborCost;
            additiancost.Reason = updateadditionalcost.Reason;
            additiancost.TotalAmount = (decimal)(updateadditionalcost.PartsCost + updateadditionalcost.LaborCost);

            additiancost.MaintenanceRequest.AdditionalCostsTotal = additiancost.TotalAmount;


            if (updateadditionalcost.ImageUrlS is not null)
            {
                var existAdditionalCostImages = await _Context.AdditionalCostImages.Where(e => e.AdditionalCostRequestId == id).ToListAsync();
                foreach (var existImg in existAdditionalCostImages)
                {
                    var oldPath = Path.Combine("wwwroot", "eadditionalcostimgs", existImg.ImageUrl);

                    if (File.Exists(oldPath))
                    {
                        File.Delete(oldPath);
                    }

                    _Context.AdditionalCostImages.Remove(existImg);
                }

                foreach (var img in updateadditionalcost.ImageUrlS)
                {


                    var FilenameadditionalcostImageUrl = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                    var filePathadditionalcostImageUrl = Path.Combine("wwwroot", "eadditionalcostimgs", FilenameadditionalcostImageUrl);
                    using (var stream = new FileStream(filePathadditionalcostImageUrl, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }
                    var additionalImags = new AdditionalCostImage
                    {
                        AdditionalCostRequestId = additiancost.Id,
                        ImageUrl = FilenameadditionalcostImageUrl,
                        CreatedAt = DateTime.UtcNow,
                    };
                    await _Context.AdditionalCostImages.AddAsync(additionalImags);
                }

            }

            var notification = new Notification
            {
                UserId = additiancost.MaintenanceRequest.CustomerId,
                Title = "Additional Cost Request Updated",
                Message = $"The additional cost request for maintenance request ({additiancost.MaintenanceRequest.RequestNumber}) has been updated." +
                          $"\nLabor Cost: {additiancost.LaborCost}" +
                          $"\nParts Cost: {additiancost.PartsCost}" +
                          $"\nTotal Amount: {additiancost.TotalAmount}" +
                          $"\nReason: {additiancost.Reason}",
                Type = NotificationType.UpdateAdditionalCostRequested,
                IsRead = false,
                RelatedEntityId = additiancost.Id.ToString(),
                CreatedAt = DateTime.UtcNow
            };

            await _Context.Notification.AddAsync(notification);
            await _Context.SaveChangesAsync();
            await _notificationService.SendToUserAsync(notification);

            return updateadditionalcost;
        }
        public async Task<bool> WorkComplete(int id, List<IFormFile> Imgs)
        {
            var notification = await _Context.Notification
                .FirstOrDefaultAsync(e => e.Id == id);

            if (notification == null)
            {
                return false;
            }

            notification.IsRead = true;

            var requestman = await _Context.MaintenanceRequests
                .Include(e => e.technicianProfileCopy)
                .Include(e => e.AssignedTechnician)
                .FirstOrDefaultAsync(e =>
                    e.RequestNumber == notification.RelatedEntityId);

            if (requestman == null)
            {
                return false;
            }

            requestman.Status = MaintenanceRequestStatus.WorkCompleted;
            requestman.CompletedAt = DateTime.UtcNow;
            requestman.technicianProfileCopy.TotalCompletedJobs += 1;

            requestman.AssignedTechnician.TotalCompletedJobs = requestman.technicianProfileCopy.TotalCompletedJobs;

            requestman.technicianProfileCopy.IsActive = true;
            requestman.AssignedTechnician.IsActive = true;


            if (Imgs is not null)
            {
                foreach (var img in Imgs)
                {
                    var FilenameFFFrontImageUrl = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                    var filePathFrontImageUrl = Path.Combine("wwwroot", "MaintenanceRequestImage", FilenameFFFrontImageUrl);
                    using (var stream = new FileStream(filePathFrontImageUrl, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }
                    var maintenanceImage = new MaintenanceRequestImage
                    {
                        MaintenanceRequestId = requestman.Id,
                        UploadedByUserId = requestman.CustomerId,
                        CreatedAt = DateTime.UtcNow,
                        IsAfterWork = true,
                        IsBeforeWork = false,
                        ImageUrl = filePathFrontImageUrl
                    };
                    await _Context.AddAsync(maintenanceImage);
                }
            }

            var noti = new Notification
            {
                UserId = requestman.CustomerId,
                Title = $"Maintenance Completed from: {requestman.AssignedTechnician.Fullname}",
                Message = $"Your maintenance request ({requestman.RequestNumber}) has been completed successfully. Please proceed with payment.",
                Type = NotificationType.WorkCompleted,
                IsRead = false,
                RelatedEntityId = requestman.RequestNumber,
                CreatedAt = DateTime.UtcNow
            };

            await _Context.Notification.AddAsync(noti);
            await _Context.SaveChangesAsync();
            await _notificationService.SendToUserAsync(noti);
            return true;
        }

        public async Task<bool> WorkCancelled(int id, string reason)
        {
            var notification = await _Context.Notification
                .FirstOrDefaultAsync(e => e.Id == id);
            if (notification == null)
            {
                return false;
            }
            notification.IsRead = true;
            var requestman = await _Context.MaintenanceRequests
                .Include(e => e.AssignedTechnician)
                .Include(e => e.technicianProfileCopy)
                .FirstOrDefaultAsync(e =>
                    e.RequestNumber == notification.RelatedEntityId);
            if (requestman == null)
            {
                return false;
            }
            requestman.Status = MaintenanceRequestStatus.Cancelled;
            var tecnician =await _Context.TechnicianProfileCopies.FirstOrDefaultAsync(e => e.Id == requestman.technicianProfileCopyId);
            tecnician.IsActive = true;

            requestman.TechnicianReport = $"Maintenance request cancelled by customer. Reason: {reason}";

            if (requestman.technicianProfileCopy != null)
            {
                requestman.technicianProfileCopy.IsActive = true;
            }
            if (requestman.AssignedTechnician != null)
            {
                requestman.AssignedTechnician.IsActive = true;
            }
            var noti = new Notification
            {
                UserId = requestman.CustomerId,
                Title = $"Maintenance Cancelled from: {requestman.AssignedTechnician.Fullname}",
                Message = $"The customer has cancelled the maintenance request ({requestman.RequestNumber}). Reason: {reason}.",
                Type = NotificationType.WorkCancelled,
                IsRead = false,
                RelatedEntityId = requestman.RequestNumber,
                CreatedAt = DateTime.UtcNow
            };
            await _Context.Notification.AddAsync(noti);
            await _Context.SaveChangesAsync();
            await _notificationService.SendToUserAsync(noti);
            return true;
        }

        public async Task<bool> Paymentcash(string requestNumber)
        {
            var maintenanceRequest = await _Context.MaintenanceRequests
                      .FirstOrDefaultAsync(e => e.RequestNumber == requestNumber && e.PaymentApproved == false && e.Status == MaintenanceRequestStatus.WorkCompleted);

            if (maintenanceRequest == null)
            {
                throw new System.Exception("Maintenance request not found.");
            }

            var tatalCost = (maintenanceRequest.FinalPrice + maintenanceRequest.AdditionalCostsTotal);
            maintenanceRequest.PaymentApproved = true;

            var payment = new Payment
            {
                MaintenanceRequestId = maintenanceRequest.Id,
                Amount = tatalCost,
                PaymentMethod = PaymentMethod.Cash,
                Status = PaymentStatus.Paid,
                CreatedAt = DateTime.UtcNow,
                PaidAt = DateTime.UtcNow
            };
            await _Context.Payment.AddAsync(payment);
            var noti = new Notification
            {
                UserId = maintenanceRequest.Company.ApplicationUserId,
                Title = $"Payment Received from: {maintenanceRequest.AssignedTechnician.Fullname}",
                Message = $"The payment for maintenance request ({maintenanceRequest.RequestNumber}) has been received in cash.",
                Type = NotificationType.PaymentSuccess,
                IsRead = false,
                RelatedEntityId = maintenanceRequest.RequestNumber,
                CreatedAt = DateTime.UtcNow
            };
            await _Context.Notification.AddAsync(noti);
            await _Context.SaveChangesAsync();
            await _notificationService.SendToUserAsync(noti);
            return true;

        }
        public async Task<bool> FinallyCompleted(int id)
        {
            var notification = await _Context.Notification
                .FirstOrDefaultAsync(e => e.Id == id);
            if (notification == null)
            {
                return false;
            }
            notification.IsRead = true;
            var requestman = await _Context.MaintenanceRequests
                .Include(e => e.AssignedTechnician)
                .Include(e=>e.Company)
                .FirstOrDefaultAsync(e =>
                    e.RequestNumber == notification.RelatedEntityId);
            if (requestman == null)
            {
                return false;
            }
            requestman.Status = MaintenanceRequestStatus.Completed;
            var tecnician = await _Context.TechnicianProfileCopies.FirstOrDefaultAsync(e => e.Id == requestman.technicianProfileCopyId);
            tecnician.IsActive = true;
            var noti = new Notification
            {
                UserId = requestman.Company.ApplicationUserId,
                Title = $"Maintenance Completed from: {requestman.AssignedTechnician.Fullname}",
                Message = $"The maintenance request ({requestman.RequestNumber}) has been marked as completed. and payment done",
                Type = NotificationType.Completed,
                IsRead = false,
                RelatedEntityId = requestman.RequestNumber,
                CreatedAt = DateTime.UtcNow
            };
            await _Context.Notification.AddAsync(noti);
            await _Context.SaveChangesAsync();
            await _notificationService.SendToUserAsync(noti);
            return true;

        }
    }
}
