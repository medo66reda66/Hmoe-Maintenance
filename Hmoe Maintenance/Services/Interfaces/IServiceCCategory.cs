using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.Models;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface IServiceCCategory
    {
        Task<ServiceCategory> CreateServiceCategory(CreateServiceCategoryRequest createServiceCategoryRequest);
        Task<IEnumerable<ServiceCategory>> GetAllServiceCategories();
        Task<ServiceCategory> GetServiceCategoryById(int id);
        Task<ServiceCategory> UpdateServiceCategory(int id, UpdateServiceCategoryRequest updateServiceCategoryRequest);
        Task<bool> DeleteServiceCategory(int id);
    }
}
