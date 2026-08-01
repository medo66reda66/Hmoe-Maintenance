using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hmoe_Maintenance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MaintenanceRequestController : ControllerBase
    {
        private readonly IMaintenanceRequestService _maintenanceRequestService;

        public MaintenanceRequestController(IMaintenanceRequestService maintenanceRequestService)
        {
            _maintenanceRequestService = maintenanceRequestService;
        }




        [HttpGet("GetAllNotificationByClient")]
        public async Task<IActionResult> GetAllNotificationByClient()
        {
            var clientid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (clientid == null)
            {
                return NotFound();
            }

            var result =await _maintenanceRequestService.GetAllNotificationToCompany(clientid);
            if (result == null)
            {
               return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMaintenance(CreateMaintenanceRequest createMaintenance)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userid == null) 
            {
                return BadRequest("user not found");
            }

            var result = await _maintenanceRequestService.createMaintenance(createMaintenance,userid);
            if (result == null)
            {
                return BadRequest("user not create Request");
            }

            return Ok(result);
        }

        [HttpPost("approve-price/{notificationId}")]
        public async Task<IActionResult> ApprovePrice(int notificationId)
        {
            var result = await _maintenanceRequestService.Approveprice(notificationId);

            if (!result)
                return NotFound(new { Message = "Notification not found." });

            return Ok(new
            {
                Message = "Price offer approved successfully."
            });
        }

        [HttpPost("reject-price/{notificationId}")]
        public async Task<IActionResult> RejectPrice(int notificationId)
        {
            var result = await _maintenanceRequestService.RejectPrice(notificationId);

            if (!result)
                return NotFound(new { Message = "Notification not found." });

            return Ok(new
            {
                Message = "Price offer rejected successfully."
            });
        }


    }
}
