using Hmoe_Maintenance.DataBase;
using Hmoe_Maintenance.Models;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Hmoe_Maintenance.Services
{
    public class AdminTechnicianService : IAdminTechnicianService
    {
        private readonly AppDBcontext _dBcontext;

        public AdminTechnicianService(AppDBcontext dBcontext)
        {
            _dBcontext = dBcontext;
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

            return true;
        }


    }
}
