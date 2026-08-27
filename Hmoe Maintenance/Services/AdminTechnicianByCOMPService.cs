using Hmoe_Maintenance.DataBase;
using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.DTOs.Response.filter;
using Hmoe_Maintenance.Models;
using Hmoe_Maintenance.Services.Interfaces;
using Hmoe_Maintenance.SignalRWebAPI;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Hmoe_Maintenance.Services
{
    public class AdminTechnicianByCOMPService : Interfaces.IAdminTechnicianByCOMPService
    {
        private readonly AppDBcontext _dBcontext;
        private readonly INotificationService _notificationService;

        public AdminTechnicianByCOMPService(AppDBcontext dBcontext, INotificationService notificationService)
        {
            _dBcontext = dBcontext;
            _notificationService = notificationService;
        }

        public async Task<PaginationResponse<TechnincianProfileResponse>> GetAllTechnicianProfiles(string compid,FilterTechnicianRequest filter,int page)
        {
            var profiles = _dBcontext.TechnicianProfiles
                .Include(t => t.Company)
                .Include(t => t.User)
                .Include(t => t.TechnicianServices)
                .Where(e=>e.Company.ApplicationUserId == compid )
                .AsNoTracking()
                .AsQueryable();

            var showTechnicianProfiles =  profiles.Select(e => new TechnincianProfileResponse
            {
                Id = e.Id,
                CompanyName = e.Company != null ? e.Company.Name : string.Empty,
                FullName = e.Fullname,
                PhoneNumber = e.User != null ? e.User.PhoneNumber! : string.Empty,
                Email = e.User != null ? e.User.Email! : string.Empty,
                NationalId = e.NationalId,
                ProfileImageUrl = e.ProfileImageUrl,
                NationalIdFrontImageUrl = e.NationalIdFrontImageUrl,
                NationalIdBackImageUrl = e.NationalIdBackImageUrl,
                TechnicianDocumentUrl = e.TechnicianDocumentUrl,
                technicianServices = e.TechnicianServices.Select(e=>e.ServiceCategory)!,
                servisecategoryname = e.TechnicianServices.Select(e => e.ServiceCategory.Name)!,
                YearsOfExperience = e.YearsOfExperience,
                Status = e.Status,
                ApprovedByUserId = e.ApprovedByUserId,
                RevenueShare = e.RevenueShare,
                Bio = e.Bio,
                AverageRating = e.AverageRating,
                TotalCompletedJobs = e.TotalCompletedJobs,
                IsAvailable = e.IsAvailable,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt
            });
            FilterTechnicianResponse filterResponse = new();

            if (filter.FullName != null)
            {
                showTechnicianProfiles = showTechnicianProfiles
                    .Where(t => t.FullName.Contains(filter.FullName));
                filterResponse.FullName = filter.FullName;
            }

            if (filter.Email != null)
            {
                showTechnicianProfiles = showTechnicianProfiles
                    .Where(t => t.Email.Contains(filter.Email));
                filterResponse.Email = filter.Email;
            }

            if (filter.NationalId != null)
            {
                showTechnicianProfiles = showTechnicianProfiles
                    .Where(t => t.NationalId == filter.NationalId);
                filterResponse.NationalId = filter.NationalId;
            }

            if (filter.CompanyName != null)
            {
                showTechnicianProfiles = showTechnicianProfiles
                    .Where(t => t.CompanyName.Contains(filter.CompanyName));
                filterResponse.CompanyName = filter.CompanyName;
            }

            if (filter.IsAvailable.HasValue)
            {
                showTechnicianProfiles = showTechnicianProfiles
                    .Where(t => t.IsAvailable == filter.IsAvailable.Value);
                filterResponse.IsAvailable = filter.IsAvailable.Value;
            }

            if (filter.IsActive.HasValue)
            {
                showTechnicianProfiles = showTechnicianProfiles
                    .Where(t => t.IsActive == filter.IsActive.Value);
                filterResponse.IsActive = filter.IsActive.Value;
            }

            if (filter.TechnicalService != null)
            {
                showTechnicianProfiles = showTechnicianProfiles
                    .Where(t => t.servisecategoryname.Contains(filter.TechnicalService));
                filterResponse.TechnicalService = filter.TechnicalService;
            }

            var Result = await PaginationService.PaginateAsync(showTechnicianProfiles, page, 5);

            return Result;
        }
        public async Task<TechnincianProfileResponse> GetTechnicianProfilesBYid(string compid, int id)
        {
            var profiles = await _dBcontext.TechnicianProfiles
                .Include(t => t.Company)
                .Include(t => t.User)
                .Include(t => t.TechnicianServices)
                .FirstOrDefaultAsync(e => e.Id == id && e.Company.ApplicationUserId == compid);

            var showProfiles =  new TechnincianProfileResponse
            {
                Id = profiles.Id,
                CompanyName = profiles.Company != null ? profiles.Company.Name : string.Empty,
                FullName = profiles.Fullname,
                PhoneNumber = profiles.User != null ? profiles.User.PhoneNumber! : string.Empty,
                Email = profiles.User != null ? profiles.User.Email! : string.Empty,
                NationalId = profiles.NationalId,
                ProfileImageUrl = profiles.ProfileImageUrl,
                NationalIdFrontImageUrl = profiles.NationalIdFrontImageUrl,
                NationalIdBackImageUrl = profiles.NationalIdBackImageUrl,
                TechnicianDocumentUrl = profiles.TechnicianDocumentUrl,
                technicianServices = profiles.TechnicianServices.Select(e => e.ServiceCategory)!,
                YearsOfExperience = profiles.YearsOfExperience,
                Status = profiles.Status,
                ApprovedByUserId = profiles.ApprovedByUserId,
                RevenueShare = profiles.RevenueShare,
                Bio = profiles.Bio,
                AverageRating = profiles.AverageRating,
                TotalCompletedJobs = profiles.TotalCompletedJobs,
                IsAvailable = profiles.IsAvailable,
                IsActive = profiles.IsActive,
                CreatedAt = profiles.CreatedAt
            };

            return showProfiles;
        }
        public async Task<bool> ApproveTechnicienCreate(int notifId)
        {
            var not =await _dBcontext.Notification
                .FirstOrDefaultAsync(e=>e.Id == notifId);

            if (not == null)
            {
                return false;
            }

            var tec = await _dBcontext.TechnicianProfiles
                .Include(e => e.Company)
                .FirstOrDefaultAsync(e => e.UserId == not.RelatedEntityId);

            if (tec == null)
            {
                return false;
            }

            var teccopy = new TechnicianProfileCopy
            {
                UserId = tec.UserId,
                CompanyId = tec.CompanyId,
                NationalId = tec.NationalId,
                Fullname = tec.Fullname,
                YearsOfExperience = tec.YearsOfExperience,
                Email = tec.Email,
                PhoneNumper = tec.PhoneNumper,
                Bio = tec.Bio,
                Status = TechnicianStatusCopy.Approved,
                IsActive = true,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow,
                ApprovedByUserId=tec.UserId,
                NationalIdBackImageUrl=tec.NationalIdBackImageUrl,
                NationalIdFrontImageUrl=tec.NationalIdFrontImageUrl,
                ProfileImageUrl=tec.ProfileImageUrl,
                TechnicianDocumentUrl = tec.TechnicianDocumentUrl
            };
            await _dBcontext.TechnicianProfileCopies.AddAsync(teccopy);

            tec.IsAvailable=true;
            tec.Status= TechnicianStatus.Approved;

            var notifyCompany = new Notification
            {
                UserId = tec.UserId,
                Title = "Application Approved",
                Message = $"Congratulations! You have been accepted to join {tec.Company.Name}.",
                Type = NotificationType.TechnicianApplicationApproved,
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
                RelatedEntityId = tec.Company.Id.ToString(),
            };

           await _dBcontext.Notification.AddAsync(notifyCompany);
            await _dBcontext.SaveChangesAsync();

            await _notificationService.SendToUserAsync(notifyCompany);
            return true;
        }
        public async Task<bool> RejectTechnicienCreate(int notifId)
        {
            var not = await _dBcontext.Notification
                .FirstOrDefaultAsync(e => e.Id == notifId);

            if (not == null)
            {
                return false;
            }

            var tec = await _dBcontext.TechnicianProfiles
                .Include(e => e.Company)
                .FirstOrDefaultAsync(e => e.UserId == not.RelatedEntityId);

            if (tec == null)
            {
                return false;
            }

            tec.Status = TechnicianStatus.Rejected;
            tec.IsAvailable = false;

            var notifyTechnician = new Notification
            {
                UserId = tec.UserId,
                Title = "Application Rejected",
                Message = $"We are sorry. Your application to join {tec.Company.Name} has been rejected.",
                Type = NotificationType.TechnicianApplicationRejected,
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
                RelatedEntityId = tec.Company.Id.ToString(),
            };

            _dBcontext.Notification.Add(notifyTechnician);

            await _dBcontext.SaveChangesAsync();
            await _notificationService.SendToUserAsync(notifyTechnician);
            return true;
        }
        public async Task<bool> ApproveTechnicianUpdate(int notifId)
        {
            var not = await _dBcontext.Notification
                .FirstOrDefaultAsync(e => e.Id == notifId);

            if (not == null)
                return false;

            var technicianCopy = await _dBcontext.TechnicianProfileCopies
                .FirstOrDefaultAsync(e => e.UserId == not.RelatedEntityId && e.IsAvailable == false);

            if (technicianCopy == null)
                return false;

            var technician = await _dBcontext.TechnicianProfiles
                .FirstOrDefaultAsync(e => e.UserId == technicianCopy.UserId);

            if (technician == null)
                return false;

            technicianCopy.Fullname = technician.Fullname;
            technicianCopy.Email = technician.Email;
            technicianCopy.PhoneNumper = technician.PhoneNumper;
            technicianCopy.NationalId = technician.NationalId;
            technicianCopy.Bio = technician.Bio;
            technicianCopy.YearsOfExperience = technician.YearsOfExperience;
            technicianCopy.CompanyId = technician.CompanyId;
            technicianCopy.IsActive = technician.IsActive;
            technicianCopy.AverageRating = technician.AverageRating;
            technicianCopy.RevenueShare = technician.RevenueShare;
            technicianCopy.ApprovedByUserId= technician.ApprovedByUserId;
            technicianCopy.TotalCompletedJobs = technician.TotalCompletedJobs;
            technicianCopy.CreatedAt = technician.CreatedAt;
            technicianCopy.ApprovedByUserId = technician.UserId;
            technicianCopy.NationalIdBackImageUrl = technician.NationalIdBackImageUrl;
            technicianCopy.NationalIdFrontImageUrl = technician.NationalIdFrontImageUrl;
            technicianCopy.ProfileImageUrl = technician.ProfileImageUrl;
            technicianCopy.TechnicianDocumentUrl = technician.TechnicianDocumentUrl;
            technicianCopy.Status = TechnicianStatusCopy.Approved;
            technicianCopy.IsAvailable = true;

            not.IsRead = true;
            not.Type = NotificationType.TechnicianApplicationApproved;

            var notification = new Notification
            {
                UserId = technician.UserId,
                Title = "Profile Update Approved",
                Message = "Your profile update has been approved successfully.",
                Type = NotificationType.TechnicianApplicationupdate,
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
                RelatedEntityId = technician.Id.ToString()
            };

            _dBcontext.Notification.Add(notification);

            await _dBcontext.SaveChangesAsync();
            await _notificationService.SendToUserAsync(notification);
            return true;
        }
        public async Task<bool> RejectTechnicianUpdate(int notifId)
        {
            var not = await _dBcontext.Notification
                .FirstOrDefaultAsync(e => e.Id == notifId);

            if (not == null)
                return false;

            var technicianCopy = await _dBcontext.TechnicianProfileCopies
                .FirstOrDefaultAsync(e => e.UserId == not.RelatedEntityId && e.Status == TechnicianStatusCopy.Pending);

            if (technicianCopy == null)
                return false;

            technicianCopy.Status = TechnicianStatusCopy.Rejected;

            var notification = new Notification
            {
                UserId = technicianCopy.UserId,
                Title = "Profile Update Rejected",
                Message = "Your profile update has been rejected.",
                Type = NotificationType.TechnicianApplicationRejected,
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
                RelatedEntityId = technicianCopy.Id.ToString()
            };

            _dBcontext.Notification.Add(notification);

            await _dBcontext.SaveChangesAsync();
            await _notificationService.SendToUserAsync(notification);
            return true;
        }
        public async Task<bool> LockUnlockTech(int id)
        {
            var tech = await _dBcontext.TechnicianProfileCopies.FirstOrDefaultAsync(e => e.Id == id);
            if (tech == null) return false;

            tech.IsAvailable = !tech.IsAvailable;

            await _dBcontext.SaveChangesAsync();
            return true;
        }



    }
}
