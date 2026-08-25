using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.Models;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface IServiceCCategory
    {
       
        Task<ServiceCategory> CreateServiceCategory(string comid, CreateServiceCategoryRequest createServiceCategoryRequest);
        Task<ServiceCategory> UpdateServiceCategory(int id, UpdateServiceCategoryRequest updateServiceCategoryRequest);
        Task<bool> DeleteServiceCategory(int id);
    }
}
