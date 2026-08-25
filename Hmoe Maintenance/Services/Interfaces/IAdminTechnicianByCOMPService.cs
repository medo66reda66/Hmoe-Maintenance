using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.Models;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface IAdminTechnicianByCOMPService
    {
        Task<PaginationResponse<TechnincianProfileResponse>> GetAllTechnicianProfiles(string compid, FilterTechnicianRequest filter, int page);

        Task<TechnincianProfileResponse> GetTechnicianProfilesBYid(string compid,int id);
        
        Task<bool> ApproveTechnicienCreate(int notifId);
        Task<bool> RejectTechnicienCreate(int notifId);

        Task<bool> ApproveTechnicianUpdate(int notifId);


        Task<bool> RejectTechnicianUpdate(int notifId);
        Task<bool> LockUnlockTech(int id);


    }
}
