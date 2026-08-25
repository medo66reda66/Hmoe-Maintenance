using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.Models;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<Company> CreateCompany(CreateCompanyRequest companyRequest, string userId);
        Task<Company> GetmyCompany(string companyId);
        Task<Company> UpdateCompany(int companyId, UpdateCompanyRequest updateCompanyRequest);
         Task<bool> DeleteCompany(int companyId);
    }   
}
