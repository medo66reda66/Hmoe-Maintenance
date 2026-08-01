using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.Models;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface ICompanyCoverageAreaService
    {
        Task<List<CompanyAreaResponse>> GetAllCompanyCoverageAreas();
        Task<CompanyAreaResponse?> GetCompanyCoverageAreaById(int id);
        Task<CompanyCoverageArea> CreateCompanyCoverageArea(CreateCompanyCoverageAreaRequest request, string userId);
        Task<CompanyCoverageArea> UpdateCompanyCoverageArea(int id,UpdateCompanyCoverageAreaRequest request);
        Task<bool> DeleteCompanyCoverageArea(int id);
    }
}
