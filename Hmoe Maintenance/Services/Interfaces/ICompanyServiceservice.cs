using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.DTOs.Response;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface ICompanyServiceservice
    {
        Task<PaginationResponse<ShowCompanyServiceResponse>> GetAllCompanyServices(FiltercompanyserviceResquest filtercompanyservice, int page);
        Task<ShowCompanyServiceResponse> GetoneCompanyServicesBYid(int id);
        Task<List<CompanyServiceResponse>> GetMYCompanyServiceById(string compid);

    }
}
