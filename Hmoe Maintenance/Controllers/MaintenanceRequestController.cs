using Ecommers.Api.Utilities;
using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hmoe_Maintenance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.CLIENT_ROLE}"))]
    public class MaintenanceRequestController : ControllerBase
    {
        private readonly IMaintenanceRequestService _maintenanceRequestService;

        public MaintenanceRequestController(IMaintenanceRequestService maintenanceRequestService)
        {
            _maintenanceRequestService = maintenanceRequestService;
        }




        [HttpGet("GetAllNotificationByClient")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.CLIENT_ROLE}"))]
        public async Task<IActionResult> GetAllNotificationByClient([FromQuery]FilternotificationRequest filternotification,int page =1)
        {
            var clientid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (clientid == null)
            {
                return NotFound();
            }

            var result =await _maintenanceRequestService.GetAllNotificationToClient(clientid,filternotification,page);
            if (result == null)
            {
               return NotFound();
            }

            return Ok(result);
        }
        [HttpGet("GetAllNotificationByClientByid/{id}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.CLIENT_ROLE}"))]
        public async Task<IActionResult> GetAllNotificationByClientById(int id)
        {
            var clientid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (clientid == null)
            {
                return NotFound();
            }

            var result = await _maintenanceRequestService.GetNotificationByclientById(id, clientid);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        [HttpGet("GetAllMaintenanceRequests")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.CLIENT_ROLE}"))]
        public async Task<IActionResult> GetAllMaintenanceRequests([FromQuery]FilterMaintenanceRequest filterMaintenance,int page = 1)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest("User not found.");
            }
            var result = await _maintenanceRequestService.GetAllMaintenanceRequestByClient(userId,filterMaintenance,page);
            if (result == null)
            {
                return NotFound("No maintenance requests found for the user.");
            }
            return Ok(result);
        }
        [HttpGet("GetAllPaymentsClient")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.CLIENT_ROLE}"))]
        public async Task<IActionResult> GetallpaymentClient()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest("User not found.");
            }
            var result = await _maintenanceRequestService.GetallPaymentByMaintenanceRequestId(userId);
            if (result == null)
            {
                return NotFound("No payment found for the user.");
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.CLIENT_ROLE}"))]
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
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.CLIENT_ROLE}"))]
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
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.CLIENT_ROLE}"))]
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

        [HttpPost("ApproveAdditionalCost/{notificationId}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.CLIENT_ROLE}"))]
        public async Task<IActionResult> ApproveAdditionalCost(int notificationId)
        {
            var result = await  _maintenanceRequestService.ApproveAdditionalCost(notificationId);

            if (!result)
            {
                return NotFound(new
                {
                    Message = "Notification or additional cost request not found."
                });
            }

            return Ok(new
            {
                Message = "Additional cost approved successfully."
            });
        }

        [HttpPost("RejectAdditionalCost/{notificationId}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.CLIENT_ROLE}"))]
        public async Task<IActionResult> RejectAdditionalCost(int notificationId,[FromBody] string Note)
        {
            var result = await _maintenanceRequestService
                .RejectAdditionalCost(notificationId, Note);

            if (!result)
            {
                return NotFound(new
                {
                    Message = "Notification or additional cost request not found."
                });
            }

            return Ok(new
            {
                Message = "Additional cost rejected successfully."
            });
        }
        [HttpPost("Review")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.CLIENT_ROLE}"))]
        public async Task<IActionResult> Review(int maintenanceRequestId, int rating, string comment)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest("User not found.");
            }
            var result = await _maintenanceRequestService.Review(maintenanceRequestId, userId, rating, comment);
            if (result == null)
            {
                return NotFound(new
                {
                    Message = "Maintenance request not found."
                });
            }
            return Ok(new
            {
                Message = "Review submitted successfully.",
                Data = result
            });
        }

    }
}
