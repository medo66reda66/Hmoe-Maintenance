using Hmoe_Maintenance.DataBase;
using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.DTOs.Response.filter;
using Hmoe_Maintenance.Models;
using Hmoe_Maintenance.Services.Interfaces;
using Hmoe_Maintenance.SignalRWebAPI;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace Hmoe_Maintenance.Services
{
    public class AdminCompanyTechService : IAdminCompanyTechService
    {
        private readonly AppDBcontext _dBcontext;
        private readonly INotificationService _notificationService;

        public AdminCompanyTechService(AppDBcontext dBcontext, INotificationService notificationService)
        {
            _dBcontext = dBcontext;
            _notificationService = notificationService;
        }

        public async Task<Notification> Sendnotification(string adminId, CreateSendNotificationRequest sendNotificationRequest)
        {
            var notification = new Notification
            {
                UserId = sendNotificationRequest.UserId,
                Title = sendNotificationRequest.Title,
                Message = sendNotificationRequest.Message,
                Type = sendNotificationRequest.Type,
                IsRead = sendNotificationRequest.IsRead,
                RelatedEntityId =adminId,
                CreatedAt = DateTime.UtcNow
            };
            _dBcontext.Notification.Add(notification);
            await _dBcontext.SaveChangesAsync();

            await _notificationService.SendToUserAsync(notification);

            return notification;
        }

        public async Task<PaginationResponse<Notification>>? GetNotification(string adminid,FilternotificationRequest filternotification,int page)
        {
            var notification =  _dBcontext.Notification
                .Where(n => n.UserId == adminid)
                .AsNoTracking()
                .OrderByDescending(e => e.CreatedAt)
                .AsQueryable();

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
                notification = notification.Where(n =>n.IsRead == filternotification.IsRead.Value);
                filternotificationRespons.Isread = filternotification.IsRead.Value;
            }

           var Result = await PaginationService.PaginateAsync(notification, page,5);

          return Result;
        }
        public async Task<Notification?> GetNotificationBYid(string adminid,int notid)
        {
            var notification = await _dBcontext.Notification
                .FirstOrDefaultAsync(n => n.Id == notid && n.UserId == adminid);

            return notification;
        }

        public async Task<PaginationResponse<Company>> GetAllCompany(FiltercompanyReqest filtercompany,int page)
        {
            var companies =  _dBcontext.Companies
                .Include(w=>w.applicationUser)
                .AsNoTracking()
                .AsQueryable();

            FiltercompanyResponse filtercompanyResponse = new FiltercompanyResponse();
            if(filtercompany.userOwnername != null)
            {
                companies = companies.Where(e => e.applicationUser.FullName.Contains(filtercompany.userOwnername.Trim()));

                filtercompanyResponse.userOwnername = filtercompany.userOwnername;
            }
            if(filtercompany.name != null)
            {
                companies = companies.Where(e=>e.Name.Contains(filtercompany.name.Trim()));
                filtercompanyResponse.Name = filtercompany.name;
            }
            if (filtercompany.email != null)
            {
                companies = companies.Where(e=>e.Email.Contains(filtercompany.email.Trim()));
                filtercompanyResponse.email = filtercompany.email;
            }
            if(filtercompany.IsApprove.HasValue)
            {
                companies = companies.Where(e => e.IsApproved == filtercompany.IsApprove.Value);
                filtercompanyResponse.IsApprove = filtercompany.IsApprove.Value;
            }
            if (filtercompany.isactive.HasValue)
            {
                companies = companies.Where(e=>e.IsActive == filtercompany.isactive.Value);
                filtercompanyResponse.isActive = filtercompany.isactive.Value;
            }

            //var totalpage = Math.Ceiling(companies.Count() / 5.0);
            //companies = companies.Skip((page-1) * 5).Take(5).ToList();
            //var currentpage = page;

            var Result =await PaginationService.PaginateAsync(companies, page, 5);

            return Result;
        }
        public async Task<Company> GetCompanyById(int companyId)
        {   
            var company = await _dBcontext.Companies
                .Include(w => w.applicationUser)
                .AsQueryable()
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == companyId);
            return company;
        }

        public async Task<PaginationResponse<CompanyAreaResponse>> GetAllCompanyCoverageAreas(FiltercompanyReqest filtercompany,int page)
        {
            var coverageAreas =  _dBcontext.CompanyCoverageAreas
                .Include(e => e.Company)
                .ThenInclude(e => e.applicationUser)
                .AsNoTracking()
                .AsQueryable();

            var showCompanyAreas = coverageAreas.Select(c => new CompanyAreaResponse
            {
                Id = c.Id,
                Governorate = c.Governorate,
                City = c.City,
                Area = c.Area,
                IsActive = c.IsActive,
                companyName = c.Company.Name,
                companyOwnerName = c.Company.applicationUser.FullName,
                DiscriptionCompany = c.Company.Description,
                phoneNumberCompany = c.Company.PhoneNumber,
                EmailCompany = c.Company.Email!
            });

            FiltercompanyResponse filtercompanyResponse = new FiltercompanyResponse();
            if (filtercompany.userOwnername != null)
            {
                showCompanyAreas = showCompanyAreas.Where(e => e.companyOwnerName.Contains( filtercompany.userOwnername.Trim()));
                filtercompanyResponse.userOwnername = filtercompany.userOwnername;
            }
            if (filtercompany.name != null)
            {
                showCompanyAreas = showCompanyAreas.Where(e => e.companyName.Contains(filtercompany.name.Trim()));
                filtercompanyResponse.Name = filtercompany.name;
            }
            if(filtercompany.Governorate != null)
            {
                showCompanyAreas = showCompanyAreas.Where(e => e.Governorate.Contains(filtercompany.Governorate.Trim()));
                filtercompanyResponse.Governorate = filtercompany.Governorate;
            }
            if(filtercompany.City != null)
            {
                showCompanyAreas = showCompanyAreas.Where(e => e.City.Contains(filtercompany.City.Trim()));
                filtercompanyResponse.City = filtercompany.City;
            }
            if(filtercompany.IsActiveArea.HasValue)
            {
                showCompanyAreas = showCompanyAreas.Where(e => e.IsActive == filtercompany.IsActiveArea.Value);
                filtercompanyResponse.IsActiveArea = filtercompany.IsActiveArea.Value;
            }

            var Result = await PaginationService.PaginateAsync(showCompanyAreas, page, 5);

            return Result;
        }
        public async Task<List<CompanyAreaResponse?>> GetCompanyCoverageAreaById(int companyid)
        {
            var coverageArea = _dBcontext.CompanyCoverageAreas
                .Include(e => e.Company)
                .ThenInclude(e => e.applicationUser)
                .AsNoTracking()
                .Where(c => c.CompanyId == companyid);

           var showCompanyAreas =await  coverageArea.Select(c => new CompanyAreaResponse
           {
               Id = c.Id,
               Governorate = c.Governorate,
               City = c.City,
               Area = c.Area,
               IsActive = c.IsActive,
               companyName = c.Company.Name,
               companyOwnerName = c.Company.applicationUser.FullName,
               DiscriptionCompany = c.Company.Description,
               phoneNumberCompany = c.Company.PhoneNumber,
               EmailCompany = c.Company.Email!
           }).ToListAsync();

            return showCompanyAreas;
        }

        public async Task<PaginationResponse<ShowTechnicianServiceResponse>> GetAllTechnicianProfiles(FilterTechnicianRequest filter,int page)
        {
            var technicianProfiles = _dBcontext.TechnicianProfileCopies
                .Include(t => t.User)
                .Include(t => t.Company)
                .Include(t => t.TechnicianServices)
                .ThenInclude(ts => ts.ServiceCategory)
                .AsNoTracking();

            var showTechnicianProfiles =  technicianProfiles.Select(t => new ShowTechnicianServiceResponse
            {
                id = t.Id,
                Emailtec = t.Email,
                Fullnametechnicia = t.Fullname,
                PhoneNumper = t.PhoneNumper,
                NationalIdtec = t.NationalId,
                ApprovedByUserId = t.ApprovedByUserId,
                Bio = t.Bio,
                tecnicalservice = t.TechnicianServices.Select(ts => ts.ServiceCategory.Name).FirstOrDefault() ?? string.Empty,
                AverageRating = t.AverageRating,
                RevenueShare = t.RevenueShare,
                TotalCompletedJobs = t.TotalCompletedJobs,
                Descriptionservicecategory = t.TechnicianServices.Select(ts => ts.ServiceCategory.Description).FirstOrDefault() ?? string.Empty,
                CompanyName = t.Company.Name,
                DescriptionCompany = t.Company.Description,
                EmailCompany = t.Company.Email,
                IsAvailable = t.IsAvailable,
                IsActive = t.IsActive,
                CreatedAt = t.CreatedAt
            });

            FilterTechnicianResponse filterResponse = new();

            if (filter.FullName != null)
            {
                showTechnicianProfiles = showTechnicianProfiles
                    .Where(t => t.Fullnametechnicia.Contains(filter.FullName));
                filterResponse.FullName = filter.FullName;
            }

            if (filter.Email != null)
            {
                showTechnicianProfiles = showTechnicianProfiles
                    .Where(t => t.Emailtec.Contains(filter.Email));
                filterResponse.Email = filter.Email;
            }

            if (filter.NationalId != null)
            {
                showTechnicianProfiles = showTechnicianProfiles
                    .Where(t => t.NationalIdtec == filter.NationalId);
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
                        .Where(t => t.tecnicalservice.Contains(filter.TechnicalService));
                    filterResponse.TechnicalService = filter.TechnicalService;
                }

                var Result = await PaginationService.PaginateAsync(showTechnicianProfiles, page, 5);

                return Result;
        }

        public async Task<ShowTechnicianServiceResponse?> GetTechnicianProfileById(int id)
        {
            var technicianProfile = await _dBcontext.TechnicianProfiles
                .Include(t => t.User)
                .Include(t => t.Company)
                .Include(t => t.TechnicianServices)
                .ThenInclude(ts => ts.ServiceCategory)
                .FirstOrDefaultAsync(t => t.Id == id);

            var showTechnicianProfile = technicianProfile == null ? null : new ShowTechnicianServiceResponse
            {
                id = technicianProfile.Id,
                Emailtec = technicianProfile.Email,
                Fullnametechnicia = technicianProfile.Fullname,
                PhoneNumper = technicianProfile.PhoneNumper,
                NationalIdtec = technicianProfile.NationalId,
                ApprovedByUserId = technicianProfile.ApprovedByUserId,
                Bio = technicianProfile.Bio,
                tecnicalservice = technicianProfile.TechnicianServices.Select(ts => ts.ServiceCategory.Name).FirstOrDefault() ?? string.Empty,
                AverageRating = technicianProfile.AverageRating,
                RevenueShare = technicianProfile.RevenueShare,
                TotalCompletedJobs = technicianProfile.TotalCompletedJobs,
                Descriptionservicecategory = technicianProfile.TechnicianServices.Select(ts => ts.ServiceCategory.Description).FirstOrDefault() ?? string.Empty,
                CompanyName = technicianProfile.Company.Name,
                DescriptionCompany = technicianProfile.Company.Description,
                EmailCompany = technicianProfile.Company.Email,
                IsAvailable = technicianProfile.IsAvailable,
                IsActive = technicianProfile.IsActive,
                CreatedAt = technicianProfile.CreatedAt
            };

            return showTechnicianProfile;
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

            await _notificationService.SendToUserAsync(notification);

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

            await _notificationService.SendToUserAsync(notification);

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

            await _notificationService.SendToUserAsync(notification);
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

            await _notificationService.SendToUserAsync(notification);

            return true;
        }

    }
}
