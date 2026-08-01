using Hmoe_Maintenance.DataBase;
using Hmoe_Maintenance.Models;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services
{
    public class AdminCompanyService : IAdminCompanyService
    {
        private readonly AppDBcontext _dBcontext;

        public AdminCompanyService(AppDBcontext dBcontext)
        {
            _dBcontext = dBcontext;
        }


        public async Task<bool>  ApproveCompanyCreate(int notid)
        {
            var notification = await _dBcontext.Notification.FirstOrDefaultAsync(e => e.Id == notid);
            if (notification == null)
            {
                return false;
            }
            var com =await _dBcontext.Companies.FirstOrDefaultAsync(e => e.ApplicationUserId == notification.RelatedEntityId); 
            if (com == null || com.IsApproved == true)
            {
                return false;
            }
                    com.IsApproved = true;
                    var comcopy = new CompanyCopy
                    {
                        ApplicationUserId = com.ApplicationUserId,
                        Name = com.Name,
                        Description = com.Description,
                        Email = com.Email,
                        IsApproved = true,
                        IsActive = com.IsActive,
                        PhoneNumber = com.PhoneNumber,
                        CommercialRegistrationNumber = com.CommercialRegistrationNumber,
                        CommercialRegistrationImageUrl = com.CommercialRegistrationImageUrl,
                        LicenseImageUrl = com.LicenseImageUrl,
                        LogoUrl = com.LogoUrl,
                        ApprovedAt = DateTime.UtcNow,
                    };
                    _dBcontext.companyCopies.Add(comcopy);

                    notification.IsRead = true;
                    notification.Type = NotificationType.CompanyApproved;

            var notifyCompany = new Notification
            {
                UserId = com.ApplicationUserId,
                Title = $"Company Approved {com.Name}",
                Message = $"Your company {com.Name} has been approved successfully.",
                Type = NotificationType.CompanyApproved,
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
                RelatedEntityId = com.Id.ToString(),
            };

            _dBcontext.Notification.Add(notifyCompany);

            await _dBcontext.SaveChangesAsync();

            return true;
        }
        public async Task<bool>  ApproveCompanyUpdate(int notid)
        {
            var notification = await _dBcontext.Notification.FirstOrDefaultAsync(e => e.Id == notid);

            if (notification == null)
            {
                return false;
            }

            var comCopy = await _dBcontext.companyCopies.FirstOrDefaultAsync(e => e.ApplicationUserId == notification.RelatedEntityId && e.IsApproved == false);
            var com = await _dBcontext.Companies.FirstOrDefaultAsync(e => e.ApplicationUserId == notification.RelatedEntityId);
            
            if (comCopy == null || com == null || comCopy.IsApproved == true)
            {
                return false;
            }

            comCopy.ApplicationUserId = com.ApplicationUserId;
            comCopy.Name = com.Name;
            comCopy.Description = com.Description;
            comCopy.Email = com.Email;
            comCopy.IsApproved = true;
            comCopy.IsActive = com.IsActive;
            comCopy.PhoneNumber = com.PhoneNumber;
            comCopy.CommercialRegistrationNumber = com.CommercialRegistrationNumber;
            comCopy.CommercialRegistrationImageUrl = com.CommercialRegistrationImageUrl;
            comCopy.LicenseImageUrl = com.LicenseImageUrl;
            comCopy.LogoUrl = com.LogoUrl;
            comCopy.ApprovedAt = DateTime.UtcNow;
            comCopy.AverageRating = com.AverageRating;
            comCopy.PhoneNumber = com.PhoneNumber;
            comCopy.ApprovedAt = com.ApprovedAt;
            comCopy.TechnicianCount = com.TechnicianCount;
            comCopy.TotalReviews = com.TotalReviews;
            comCopy.CompletedRequestsCount = com.CompletedRequestsCount;
            comCopy.CreatedAt = com.CreatedAt;
            
             
            comCopy.IsApproved = true;

                    notification.IsRead = true;
                    notification.Type = NotificationType.CompanyApproved;

            var notifyCompany = new Notification
            {
                UserId = comCopy.ApplicationUserId,
                Title = $"Company Approved {comCopy.Name}",
                Message = $"Your company {comCopy.Name} has been approved Update successfully.",
                Type = NotificationType.CompanyApproved,
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
                RelatedEntityId = comCopy.Id.ToString(),
            };

            _dBcontext.Notification.Add(notifyCompany);

            await _dBcontext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RejectCompanyCreate(int notificationId)
        {
            var notification = await _dBcontext.Notification
                .FirstOrDefaultAsync(e => e.Id == notificationId);

            if (notification == null)
                return false;

            var company = await _dBcontext.Companies
                .FirstOrDefaultAsync(e => e.ApplicationUserId == notification.RelatedEntityId);

            if (company == null)
                return false;

            notification.IsRead = true;
            notification.Type = NotificationType.CompanyRejected;
            

            var notifyCompany = new Notification
            {
                UserId = company.ApplicationUserId,
                Title = $"Create Company Rejected - {company.Name}",
                Message = $"Your company '{company.Name}' has been rejected.",
                Type = NotificationType.CompanyRejected,
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
                RelatedEntityId = company.Id.ToString(),
            };

            _dBcontext.Notification.Add(notifyCompany);

            await _dBcontext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RejectCompanyUpdate(int notificationId)
        {
            var notification = await _dBcontext.Notification
                .FirstOrDefaultAsync(e => e.Id == notificationId);

            if (notification == null)
                return false;

            var company = await _dBcontext.companyCopies
                .FirstOrDefaultAsync(e => e.ApplicationUserId == notification.RelatedEntityId);

            if (company == null)
                return false;

            notification.IsRead = true;
            notification.Type = NotificationType.CompanyRejected;

            var notifyCompany = new Notification
            {
                UserId = company.ApplicationUserId,
                Title = $"Update Company Rejected - {company.Name}",
                Message = $"Your company '{company.Name}' has been rejected.",
                Type = NotificationType.CompanyRejected,
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
                RelatedEntityId = company.Id.ToString(),
            };

            _dBcontext.Notification.Add(notifyCompany);

            await _dBcontext.SaveChangesAsync();

            return true;
        }
    }
}
