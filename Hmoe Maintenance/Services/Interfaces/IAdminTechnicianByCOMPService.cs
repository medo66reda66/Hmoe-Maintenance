using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.DTOs.Response.filter;
using Hmoe_Maintenance.Models;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface IAdminTechnicianByCOMPService
    {
        Task<PaginationResponse<TechnincianProfileResponse,FilterTechnicianResponse>> GetAllTechnicianProfiles(string compid, FilterTechnicianRequest filter, int page);

        Task<TechnincianProfileResponse> GetTechnicianProfilesBYid(string compid,int id);

        Task<PaginationResponse<ShowTechpayout,FilterTechPayoutResponse>> GetTechpayout(string compid, FilterTechPayoutRequest filter, int page);
        Task<bool> ApproveTechnicienCreate(int notifId);
        Task<bool> RejectTechnicienCreate(int notifId);

        Task<bool> ApproveTechnicianUpdate(int notifId);

          Task<TechnicianProfileCopy?> CreateRevenueShare(
                    int techId,
                    decimal revenueShare);


          Task<TechnicianProfileCopy?> UpdateRevenueShare(
            int techId,
            decimal revenueShare);
        
            Task<bool> RejectTechnicianUpdate(int notifId);
        Task<bool> LockUnlockTech(int id);
        Task<TechnicianPayout?> TechnicianPayout(string nationalId, string notes);


    }
}
