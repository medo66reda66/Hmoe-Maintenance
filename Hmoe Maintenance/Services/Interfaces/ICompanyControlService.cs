using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.Models;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface ICompanyControlService
    {
        Task<PaginationResponse<Notification>> GetAllNotificationToCompany(string userid, FilternotificationRequest filternotification, int page);
        Task<Notification> GetAllNotificationBycompanyById(int id, string comid);
        Task<PaginationResponse<PaymentResponse>> GetAllPaymentbyClient(string comid, FilterclientRequest filter, int page);
        Task<bool> ApprovecompanyRequest(int notifid);
        Task<bool> RejectCompanyRequest(int notifid);
        Task<CreatepriceRequest> Createprisebycompany(int notifid, CreatepriceRequest createprise);
        Task<bool> AssignedTechnicianRequest(int id, int Tecid);
    }
}
