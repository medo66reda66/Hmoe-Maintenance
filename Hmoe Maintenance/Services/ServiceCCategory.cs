using Hmoe_Maintenance.DataBase;
using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.Models;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Hmoe_Maintenance.Services
{
    public class ServiceCCategory : IServiceCCategory
    {
        private readonly AppDBcontext _context;

        public ServiceCCategory(AppDBcontext context)
        {
            _context = context;
        }

        //AdminANDCompany
       
        public async Task<ServiceCategory> CreateServiceCategory(string comid, CreateServiceCategoryRequest CreateserviceCategory)
        {
            var serviceCategory = new ServiceCategory
            {
                Name = CreateserviceCategory.Name,
                Description = CreateserviceCategory.Description,
                IsActive = CreateserviceCategory.IsActive,
            };

            if (CreateserviceCategory.IconUrl != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(CreateserviceCategory.IconUrl.FileName);
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ServiceUrl", fileName);

                using (var stream = System.IO.File.Create(uploadsFolder))
                {
                    await CreateserviceCategory.IconUrl.CopyToAsync(stream);
                }

                serviceCategory.IconUrl = fileName;
            }
            await _context.ServiceCategories.AddAsync(serviceCategory);
            await _context.SaveChangesAsync();

            
            if (CreateserviceCategory.CompanyServiceInspectionPrice != null && CreateserviceCategory.CompanyServiceStartingPrice != null
                && CreateserviceCategory.CompanyServicecoDescription != null && CreateserviceCategory.CompanyServiceIsActive != null)
            {
                var company = await _context.Companies.FirstOrDefaultAsync(c => c.ApplicationUserId == comid);

                if (company == null)
                {
                    return null;
                }
                var companyService = new Models.CompanyService
                {
                    ServiceCategoryId = serviceCategory.Id,
                    CompanyId = company.Id,
                    InspectionPrice = (decimal)CreateserviceCategory.CompanyServiceInspectionPrice,
                    StartingPrice = CreateserviceCategory.CompanyServiceStartingPrice,
                    Description = CreateserviceCategory.CompanyServicecoDescription,
                    IsActive = (bool)CreateserviceCategory.CompanyServiceIsActive
                };
                await _context.CompanyServices.AddAsync(companyService);
            }
           
            await _context.SaveChangesAsync();

            return serviceCategory;
        }
        public async Task<ServiceCategory> UpdateServiceCategory(int id, UpdateServiceCategoryRequest updateServiceCategory)
        {
            var serviceCategory = await _context.ServiceCategories.FirstOrDefaultAsync(e => e.Id == id);
            var companyservice = await _context.CompanyServices.FirstOrDefaultAsync(c => c.ServiceCategoryId == id);

            serviceCategory.Name =
                updateServiceCategory.Name ?? serviceCategory.Name;
            serviceCategory.Description =
                updateServiceCategory.Description ?? serviceCategory.Description;
            serviceCategory.IsActive =
                updateServiceCategory.IsActive ?? serviceCategory.IsActive;
            companyservice.InspectionPrice = 
                updateServiceCategory.CompanyServiceInspectionPrice??companyservice.InspectionPrice;
            companyservice.StartingPrice = 
                updateServiceCategory.CompanyServiceStartingPrice??companyservice.StartingPrice;
            companyservice.Description =
                updateServiceCategory.CompanyServicecoDescription??companyservice.Description;
            companyservice.IsActive = 
                updateServiceCategory.CompanyServiceIsActive??companyservice.IsActive;
            if (updateServiceCategory.IconUrl != null)
            {
                var existingFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ServiceUrl", serviceCategory.IconUrl);
                if (System.IO.File.Exists(existingFilePath))
                {
                    System.IO.File.Delete(existingFilePath);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(updateServiceCategory.IconUrl.FileName);
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ServiceUrl", fileName);
                using (var stream = System.IO.File.Create(uploadsFolder))
                {
                    await updateServiceCategory.IconUrl.CopyToAsync(stream);
                }
                serviceCategory.IconUrl = fileName;
            }
            else
            {
                serviceCategory.IconUrl = serviceCategory.IconUrl;
            }


            _context.ServiceCategories.Update(serviceCategory);
            await _context.SaveChangesAsync();

            return serviceCategory;
        }
        public async Task<bool> DeleteServiceCategory(int id)
        {
            var serviceCategory = await _context.ServiceCategories.FirstOrDefaultAsync(e => e.Id == id);
            var companyService = await _context.CompanyServices.FirstOrDefaultAsync(c => c.ServiceCategoryId == id);
            if (serviceCategory == null)
            {
                return false;
            }
            if (serviceCategory.IconUrl != null)
            {
                var existingFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ServiceUrl", serviceCategory.IconUrl);
                if (System.IO.File.Exists(existingFilePath))
                {
                    System.IO.File.Delete(existingFilePath);
                }
            }
            _context.ServiceCategories.Remove(serviceCategory);
            if (companyService != null)
            {
                _context.CompanyServices.Remove(companyService);
            }
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
