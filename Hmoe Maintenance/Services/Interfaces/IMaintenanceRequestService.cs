using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.Models;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface IMaintenanceRequestService
    {
        Task<PaginationResponse<Notification>> GetAllNotificationToClient(string clientid, FilternotificationRequest filternotification, int page );

        Task<Notification> GetNotificationByclientById(int id, string clienid);
        Task<PaginationResponse<MaintenanceRequestResponse>> GetAllMaintenanceRequestByClient(string clientid, FilterMaintenanceRequest filter, int page);
        Task<List<Payment>> GetallPaymentByMaintenanceRequestId(string clientid);
           Task<CreateMaintenanceRequest> createMaintenance(CreateMaintenanceRequest createMaintenanceRequest, string userid);
            Task<bool> Approveprice(int notificationId);

            Task<bool> RejectPrice(int notificationId);
            Task<bool> ApproveAdditionalCost(int notificationId);

            Task<bool> RejectAdditionalCost(int notificationId, string? note);
        Task<Models.Review> Review(int maintenanceRequestId, string userId, int rating, string comment);


    }
}
