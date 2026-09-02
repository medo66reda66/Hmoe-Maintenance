using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.DTOs.Response.filter;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface ICompanyServiceservice
    {
        Task<PaginationResponse<ShowCompanyServiceResponse,FiltercompanyserviceResponse>> GetAllCompanyServices(FiltercompanyserviceResquest filtercompanyservice, int page);
        Task<ShowCompanyServiceResponse> GetoneCompanyServicesBYid(int id);
        Task<List<CompanyServiceResponse>> GetMYCompanyServiceById(string compid);
        Task<ShowCompanyServiceResponse> CreateServiceTomyCompany(string compid, CreateCompanyServiceRequest createCompanyService);

    }
}
