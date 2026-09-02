using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.DTOs.Response.filter;
using Hmoe_Maintenance.Models;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface ICompanyProfileAndDetailsService
     {
        Task<PaginationResponse<ServiceCategory,FilterServiceCategoryResponse>> GetAllServiceCategories(FilterServiceCategoryRequest filter, int page);
        Task<CompanyProfileResponse> GetCompanyProfileAndDetailsServiceById(int companyId);
        Task<PaginationResponse<CompanyProfileResponse,FilterCompanyProfileResponse>> AllCompanyProfileAndDetailsService(int serviceid, FilterCompanyProfileRequest filter, int page);
         Task<List<ToptencompanyResponse>> Gettoptencompany();
    }
}
