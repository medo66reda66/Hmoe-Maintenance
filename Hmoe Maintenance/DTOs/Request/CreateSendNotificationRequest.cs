using Hmoe_Maintenance.Models;

namespace Hmoe_Maintenance.DTOs.Request
{
    public class CreateSendNotificationRequest
    {
        public string UserId { get; set; } = default!;

        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;

        public NotificationType Type { get; set; }

        public bool IsRead { get; set; }

    }
}
