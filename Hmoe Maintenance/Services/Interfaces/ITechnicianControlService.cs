using Hmoe_Maintenance.Models;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface ITechnicianControlService
    {
        Task<List<Notification>> GetAllNotificationByTech(string techid);

        Task<Notification> GetAllNotificationByTechById(int id, string techid);
        
            Task<bool> CreateselectTime(int id, TimeSpan Time);

        Task<bool> UpdateSelectTime(int notificationId, TimeSpan? time);

        Task<bool> TechnicianOnTheWay(int id);

        Task<bool> TechnicianArrived(int id);

        Task<bool> WorkStarted(int id, string Tecid);
        
    }
}
