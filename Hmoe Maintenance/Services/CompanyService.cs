using Hmoe_Maintenance.DataBase;
using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.Exception;
using Hmoe_Maintenance.Models;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly AppDBcontext _context;
        private readonly IAdminCompanyTechService _adminCompanyService;
        private readonly UserManager<ApplicationUser> _userManager;
        public CompanyService(AppDBcontext context, UserManager<ApplicationUser> userManager, IAdminCompanyTechService adminCompanyService)
        {
            _context = context;
            _userManager = userManager;
            _adminCompanyService = adminCompanyService;
        }
        public async Task<Company> GetmyCompany(string companyId)
        {
            var company = await _context.Companies
                .Include(e => e.applicationUser)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == companyId);

            return company;
        }
        public async Task<Company> CreateCompany(CreateCompanyRequest companyRequest, string userId)
        {
            var checkUser = await _context.Companies.FirstOrDefaultAsync(c => c.ApplicationUserId == userId);
            if (checkUser != null)
            {
                return null;

            }
                var company = new Company
                {
                    ApplicationUserId = userId,
                    Name = companyRequest.Name,
                    Description = companyRequest.Description,
                    Email = companyRequest.Email,
                    IsApproved = false,
                    IsActive = companyRequest.IsActive,
                    PhoneNumber = companyRequest.PhoneNumber,
                    CommercialRegistrationNumber = companyRequest.CommercialRegistrationNumber,
                };

                if (companyRequest.LogoUrl != null && companyRequest.LogoUrl.Length > 0)
                {
                    var logoFileName = Guid.NewGuid().ToString() + Path.GetExtension(companyRequest.LogoUrl.FileName);
                    var logoFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\logosCompany", logoFileName);
                    using (var stream = System.IO.File.Create(logoFilePath))
                    {
                        await companyRequest.LogoUrl.CopyToAsync(stream);
                    }
                    company.LogoUrl = logoFileName;
                }

                if(companyRequest.CommercialRegistrationImageUrl != null)
                {
                    var CommercialFileName = Guid.NewGuid().ToString() + Path.GetExtension(companyRequest.CommercialRegistrationImageUrl.FileName);
                    var CommercialFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CommercialRegistrationImageUrl", CommercialFileName);
                    using (var stream = System.IO.File.Create(CommercialFilePath))
                    {
                        await companyRequest.CommercialRegistrationImageUrl.CopyToAsync(stream);
                    }
                    company.CommercialRegistrationImageUrl = CommercialFileName;
                }

                if(companyRequest.LicenseImageUrl != null)
                {
                var LicenseFileName = Guid.NewGuid().ToString() + Path.GetExtension(companyRequest.LicenseImageUrl.FileName);
                var LicenseFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\LicenseImageUrl", LicenseFileName);
                using (var stream = System.IO.File.Create(LicenseFilePath))
                {
                    await companyRequest.LicenseImageUrl.CopyToAsync(stream);
                }
                company.LicenseImageUrl = LicenseFileName;
                }

            await _context.AddAsync(company);
            await _context.SaveChangesAsync();

            var useradmin = await _userManager.GetUsersInRoleAsync("Admin");
            foreach (var user in useradmin)
            {
                var notification = new Notification
                {
                    UserId = user.Id!,
                    Title = "Add new Company",
                    Message = $"{companyRequest.Name} Maintenance requested approval {companyRequest.Description}",
                    CreatedAt = DateTime.Now,
                    IsRead = false,
                    Type = NotificationType.CompanyPendingApproval,
                    RelatedEntityId = company.ApplicationUserId,
                };
               _context.Notification.Add(notification);
            }
            await _context.SaveChangesAsync();

            return company;
        }

        public async Task<Company> UpdateCompany(int companyId, UpdateCompanyRequest updateCompanyRequest)
        {

            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == companyId);
            if(company is null)
            {
                return null;
            }

            var pendingUpdate = await _context.companyCopies
                .FirstOrDefaultAsync(c =>
             c.ApplicationUserId == company.ApplicationUserId && c.IsApproved == true);

            if (pendingUpdate == null)
            {
                return null;
            }

            company.Name = updateCompanyRequest.Name;
            company.Description = updateCompanyRequest.Description;
            company.Email = updateCompanyRequest.Email;
            company.IsActive = updateCompanyRequest.IsActive;
            company.PhoneNumber = updateCompanyRequest.PhoneNumber;
            company.CommercialRegistrationNumber = updateCompanyRequest.CommercialRegistrationNumber;
            pendingUpdate.IsApproved = false;

            if (updateCompanyRequest.LogoUrl != null && updateCompanyRequest.LogoUrl.Length > 0)
            {
                var existingLogoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\logosCompany", company.LogoUrl);
                if (System.IO.File.Exists(existingLogoPath))
                {
                    System.IO.File.Delete(existingLogoPath);
                }

                var logoFileName = Guid.NewGuid().ToString() + Path.GetExtension(updateCompanyRequest.LogoUrl.FileName);
                var logoFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\logosCompany", logoFileName);
                using (var stream = System.IO.File.Create(logoFilePath))
                {
                    await updateCompanyRequest.LogoUrl.CopyToAsync(stream);
                }
                company.LogoUrl = logoFileName;
            }
            else
            {
                company.LogoUrl = company.LogoUrl;
            }

            if (updateCompanyRequest.CommercialRegistrationImageUrl != null)
            {
                var commercialLogoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CommercialRegistrationImageUrl", company.CommercialRegistrationImageUrl);
                if (System.IO.File.Exists(commercialLogoPath))
                {
                    System.IO.File.Delete(commercialLogoPath);
                }

                var commercialFileName = Guid.NewGuid().ToString() + Path.GetExtension(updateCompanyRequest.LogoUrl.FileName);
                var commercialFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CommercialRegistrationImageUrl", commercialFileName);
                using (var stream = System.IO.File.Create(commercialFilePath))
                {
                    await updateCompanyRequest.LogoUrl.CopyToAsync(stream);
                }
                company.LogoUrl = commercialFileName;
            }
            else { company.LogoUrl = company.LogoUrl; }

            if (updateCompanyRequest.LicenseImageUrl != null)
            {
                var LicenseLogoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\LicenseImageUrl", company.LicenseImageUrl);
                if (System.IO.File.Exists(LicenseLogoPath))
                {
                    System.IO.File.Delete(LicenseLogoPath);
                }

                var LicenseFileName = Guid.NewGuid().ToString() + Path.GetExtension(updateCompanyRequest.LicenseImageUrl.FileName);
                var LicenseFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\LicenseImageUrl", LicenseFileName);
                using (var stream = System.IO.File.Create(LicenseFilePath))
                {
                    await updateCompanyRequest.LicenseImageUrl.CopyToAsync(stream);
                }
                company.LogoUrl = LicenseFileName;
            }
            else { company.LogoUrl = company.LicenseImageUrl; }

            if (updateCompanyRequest.LicenseImageUrl != null || updateCompanyRequest.CommercialRegistrationNumber != null || updateCompanyRequest.CommercialRegistrationImageUrl != null)
            {
                var useradmin = await _userManager.GetUsersInRoleAsync("Admin");
                foreach (var user in useradmin)
                {
                    var notification = new Notification
                    {
                        UserId = user.Id!,
                        Title = "update new Company",
                        Message = $"{updateCompanyRequest.Name} Maintenance requested approval {updateCompanyRequest.Description}",
                        CreatedAt = DateTime.Now,
                        IsRead = false,
                        Type = NotificationType.CompanyPendingApproval,
                        RelatedEntityId = company.ApplicationUserId,
                    };
                    _context.Notification.Add(notification);
                }
            }
            ////////////////
            //////
            //////
            ///
            _context.Companies.Update(company);
                await _context.SaveChangesAsync();
         
                return company;
        }

        public async Task<bool> DeleteCompany(int companyId)
        {
           var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == companyId);
           if (company == null)
           {
               return false;
           }

           if (company.LogoUrl != null)
           {
               var logoFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\logosCompany", company.LogoUrl);
               if (System.IO.File.Exists(logoFilePath))
               {
                   System.IO.File.Delete(logoFilePath);
               }
           }
           if (company.CommercialRegistrationImageUrl != null)
           {
               var CommercialFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\CommercialRegistrationImageUrl", company.CommercialRegistrationImageUrl);
               if (System.IO.File.Exists(CommercialFilePath))
               {
                   System.IO.File.Delete(CommercialFilePath);
               }
           }
           if (company.LicenseImageUrl != null)
           {
               var LicenseFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\LicenseImageUrl", company.LicenseImageUrl);
               if (System.IO.File.Exists(LicenseFilePath))
               {
                   System.IO.File.Delete(LicenseFilePath);
               }
           }
  
           _context.Companies.Remove(company);
           await _context.SaveChangesAsync();
           return true;
       }
    }
}
