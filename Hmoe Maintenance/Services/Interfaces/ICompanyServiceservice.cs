using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Response;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface ICompanyServiceservice
    {
        Task<IEnumerable<CompanyServiceResponse>> GetAllCompanyServices();

        Task<CompanyServiceResponse?> GetCompanyServiceById(int id);

        Task<Models.CompanyService?> CreateCompanyService(CreateCompanyServiceRequest request, string applicationUserId);
        
        Task<Models.CompanyService?> UpdateCompanyService(int id, UpdateCompanyServiceRequest request);

        Task<bool> DeleteCompanyService(int id);
    }
}
