using Hmoe_Maintenance.Models;
using Microsoft.AspNetCore.SignalR;

namespace Hmoe_Maintenance.SignalRWebAPI
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task SendToUserAsync(Notification notification)
        {

            await _hubContext.Clients.Group($"user-{notification.UserId}")
                  .SendAsync("ReceiveNotification", notification);
        }
    }
}
