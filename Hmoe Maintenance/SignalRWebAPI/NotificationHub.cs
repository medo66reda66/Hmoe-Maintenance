using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Hmoe_Maintenance.SignalRWebAPI
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {

            var userId = Context.GetHttpContext()?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(
                Context.ConnectionId,
                $"user-{userId}");
            }
            await base.OnConnectedAsync();
        }
    }
}

