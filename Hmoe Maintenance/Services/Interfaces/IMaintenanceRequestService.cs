using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.Models;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface IMaintenanceRequestService
    {
        Task<List<Notification>> GetAllNotificationToCompany(string clientid);
            Task<CreateMaintenanceRequest> createMaintenance(CreateMaintenanceRequest createMaintenanceRequest, string userid);
            Task<bool> Approveprice(int notificationId);

            Task<bool> RejectPrice(int notificationId);
            Task<bool> ApproveAdditionalCost(int notificationId);

            Task<bool> RejectAdditionalCost(int notificationId, string? note);
        
    }
}
