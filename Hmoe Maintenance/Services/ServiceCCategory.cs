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

        public async Task<IEnumerable<ServiceCategory>> GetAllServiceCategories()
        {
            var serviceCategories = await _context.ServiceCategories.AsQueryable().ToListAsync();

            return serviceCategories;
        }
        public async Task<ServiceCategory> GetServiceCategoryById(int id)
        {
            var serviceCategory = await _context.ServiceCategories.FirstOrDefaultAsync(e => e.Id == id);
            return serviceCategory;
        }
        public async Task<ServiceCategory> CreateServiceCategory(CreateServiceCategoryRequest CreateserviceCategory)
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

            return serviceCategory;
        }
        public async Task<ServiceCategory> UpdateServiceCategory(int id, UpdateServiceCategoryRequest updateServiceCategory)
        {
            var serviceCategory = await _context.ServiceCategories.FindAsync(id);

            serviceCategory.Name = updateServiceCategory.Name;
            serviceCategory.Description = updateServiceCategory.Description;
            serviceCategory.IsActive = updateServiceCategory.IsActive;
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
            var serviceCategory = await _context.ServiceCategories.FindAsync(id);
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
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
