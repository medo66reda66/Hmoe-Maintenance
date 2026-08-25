using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.DTOs.Response;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface ILockunlockUserService
    {
        Task<PaginationResponse<UsersResponse>> Index(FilterUsersRequest filter, int page);
        Task<bool> Loukunlouk(string userid);

    }
}
