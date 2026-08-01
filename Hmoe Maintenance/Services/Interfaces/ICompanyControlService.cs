using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.Models;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface ICompanyControlService
    {
        Task<List<Notification>> GetAllNotificationToCompany(string userid);
        Task<Notification> GetAllNotificationBycompanyById(int id, string comid);
        Task<bool> ApprovecompanyRequest(int notifid);
        Task<bool> RejectCompanyRequest(int notifid);
        Task<CreatepriceRequest> Createprisebycompany(int notifid, CreatepriceRequest createprise);
        Task<bool> AssignedTechnicianRequest(int id, int Tecid);
    }
}
