using Ecommers.Api.Utilities;
using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.Services;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hmoe_Maintenance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.TECHNICAL_ROLE}"))]
    public class TechnicianController : ControllerBase
    {

        private readonly ITechnicianControlService _technicianControlService;

        public TechnicianController(ITechnicianControlService technicianControlService)
        {
            _technicianControlService = technicianControlService;
        }

        [HttpGet("allnotifications")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.TECHNICAL_ROLE}"))]
        public async Task<IActionResult> GetAllNotificationByTech([FromQuery]FilternotificationRequest filternotification,int page=1)
        {
            var technicianId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (technicianId == null)
            {
                return NotFound();
            }

            var notifications = await _technicianControlService.GetAllNotificationByTech(technicianId,filternotification,page);

            if (notifications == null)
                return NotFound("No notifications found.");

            return Ok(notifications);
        }

        [HttpGet("notificationsByid/{notificationId}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.TECHNICAL_ROLE}"))]
        public async Task<IActionResult> GetAllNotificationByTechById(int notificationId)
        {
            var technicianId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (technicianId == null)
            {
                return NotFound();
            }
            var notification = await _technicianControlService.GetAllNotificationByTechById(notificationId, technicianId);

            if (notification == null)
                return NotFound("Notification not found.");

            return Ok(notification);
        }

        [HttpPut("create-select-time/{notificationId}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.TECHNICAL_ROLE}"))]
        public async Task<IActionResult> CreateSelectTime(int notificationId, [FromBody] TimeSpan time)
        {
            var result = await _technicianControlService.CreateselectTime(notificationId, time);

            if (!result)
                return BadRequest("Failed to create appointment time.");

            return Ok("Appointment time created successfully.");
        }

        [HttpPut("update-select-time/{notificationId}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.TECHNICAL_ROLE}"))]
        public async Task<IActionResult> UpdateSelectTime(int notificationId, [FromBody] TimeSpan? time)
        {
            var result = await _technicianControlService.UpdateSelectTime(notificationId, time);

            if (!result)
                return BadRequest("Failed to update appointment time.");

            return Ok("Appointment time updated successfully.");
        }

        [HttpPut("technician-on-the-way/{notificationId}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.TECHNICAL_ROLE}"))]
        public async Task<IActionResult> TechnicianOnTheWay(int notificationId)
        {
            var result = await _technicianControlService.TechnicianOnTheWay(notificationId);

            if (!result)
                return BadRequest("Failed to update technician status.");

            return Ok("Technician is on the way.");
        }

        [HttpPut("technician-arrived/{notificationId}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.TECHNICAL_ROLE}"))]
        public async Task<IActionResult> TechnicianArrived(int notificationId)
        {
            var result = await _technicianControlService.TechnicianArrived(notificationId);

            if (!result)
                return BadRequest("Failed to update technician status.");

            return Ok("Technician has arrived.");
        }

        [HttpPut("work-started/{notificationId}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.TECHNICAL_ROLE}"))]
        public async Task<IActionResult> WorkStarted(int notificationId)
        {
            var technicianId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (technicianId == null)
            {
                return NotFound();
            }

            var result = await _technicianControlService.WorkStarted(notificationId, technicianId);

            if (!result)
                return BadRequest("Failed to start work.");

            return Ok("Work started successfully.");
        }

        [HttpPost("AdditionalCost/{notificationId}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.TECHNICAL_ROLE}"))]
        public async Task<IActionResult> AdditionalCost(int notificationId, CreateadditionalcostRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _technicianControlService.AdditionalCost(notificationId, request);

            if (result == null)
                return NotFound(new
                {
                    Message = "Notification or maintenance request not found."
                });

            return Ok(new
            {
                Message = "Additional cost request created successfully.",
                Data = result
            });
        }
        [HttpPut("UpdateAdditionalCost/{additionalCostId}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.TECHNICAL_ROLE}"))]
        public async Task<IActionResult> UpdateAdditionalCost(int additionalCostId, UpdateadditionalcostRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _technicianControlService.Updateadditionalcost(additionalCostId, request);

            if (result == null)
                return NotFound(new
                {
                    Message = "Additional cost request not found."
                });

            return Ok(new
            {
                Message = "Additional cost request updated successfully.",
                Data = result
            });
        }

        [HttpPost("WorkComplete/{notificationId}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.TECHNICAL_ROLE}"))]
        public async Task<IActionResult> WorkComplete(int notificationId, [FromForm] List<IFormFile> images)
        {
            var result = await _technicianControlService.WorkComplete(notificationId, images);

            if (!result)
                return NotFound(new
                {
                    Message = "Notification or maintenance request not found."
                });

            return Ok(new
            {
                Message = "Work completed successfully and customer has been notified."
            });
        }

        [HttpPost("PaymentCash/{requestNumber}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.TECHNICAL_ROLE}"))]
        public async Task<IActionResult> PaymentCash(string requestNumber)
        {
            var result = await _technicianControlService.Paymentcash(requestNumber);
            if (!result)
                return NotFound(new
                {
                    Message = "Notification or maintenance request not found."
                });
            return Ok(new
            {
                Message = "Payment marked as cash successfully and customer has been notified."
            });
        }

        [HttpPost("WorkCancelled/{notificationId}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.TECHNICAL_ROLE}"))]
        public async Task<IActionResult> WorkCancelled(int notificationId, [FromBody] string reason)
        {
            var result = await _technicianControlService.WorkCancelled(notificationId, reason);
            if (!result)
                return NotFound(new
                {
                    Message = "Notification or maintenance request not found."
                });
            return Ok(new
            {
                Message = "Work cancelled successfully and customer has been notified."
            });
        }

        [HttpPost("FinallyCompleted/{notificationId}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.TECHNICAL_ROLE}"))]
        public async Task<IActionResult> FinallyCompleted(int notificationId)
        {
            var result = await _technicianControlService.FinallyCompleted(notificationId);
            if (!result)
                return NotFound(new
                {
                    Message = "Notification or maintenance request not found."
                });
            return Ok(new
            {
                Message = "Maintenance request marked as finally completed."
            });
        }
    }
}

