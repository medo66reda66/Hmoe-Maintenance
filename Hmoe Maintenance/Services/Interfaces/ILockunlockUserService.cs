using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.DTOs.Response.filter;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface ILockunlockUserService
    {
        Task<PaginationResponse<UsersResponse,FilterUsersResponse>> Index(FilterUsersRequest filter, int page);
        Task<bool> Loukunlouk(string userid);

    }
}
