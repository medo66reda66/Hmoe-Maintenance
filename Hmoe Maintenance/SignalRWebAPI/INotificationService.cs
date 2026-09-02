using Hmoe_Maintenance.Models;

namespace Hmoe_Maintenance.SignalRWebAPI
{
    public interface INotificationService
    {
        Task SendToUserAsync(Notification notification);
    }
}
