using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.Models;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface ITechnicianControlService
    {
        Task<PaginationResponse<Notification>> GetAllNotificationByTech(string techid, FilternotificationRequest filternotification, int page);

        Task<Notification> GetAllNotificationByTechById(int id, string techid);
        
        Task<bool> CreateselectTime(int id, TimeSpan Time);

        Task<bool> UpdateSelectTime(int notificationId, TimeSpan? time);

        Task<bool> TechnicianOnTheWay(int id);

        Task<bool> TechnicianArrived(int id);

        Task<bool> WorkStarted(int id, string Tecid);

        Task<CreateadditionalcostRequest> AdditionalCost(int id, CreateadditionalcostRequest createadditionalcost);

        Task<UpdateadditionalcostRequest> Updateadditionalcost(int id, UpdateadditionalcostRequest updateadditionalcost);

        Task<bool> WorkComplete(int id, List<IFormFile> Imgs);

         Task<bool> WorkCancelled(int id, string reason);

        Task<bool> Paymentcash(string requestNumber);
         Task<bool> FinallyCompleted(int id);
      
    }
}
