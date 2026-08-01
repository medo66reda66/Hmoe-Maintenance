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
    [Authorize]
    public class TechnicianController : ControllerBase
    {

        private readonly ITechnicianControlService _technicianControlService;

        public TechnicianController(ITechnicianControlService technicianControlService)
        {
            _technicianControlService = technicianControlService;
        }

        [HttpGet("allnotifications")]
        public async Task<IActionResult> GetAllNotificationByTech()
        {
            var technicianId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(technicianId == null)
            {
                return NotFound();
            }

            var notifications = await _technicianControlService.GetAllNotificationByTech(technicianId);

            if (notifications == null)
                return NotFound("No notifications found.");

            return Ok(notifications);
        }

        [HttpGet("notificationsByid/{notificationId}")]
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
        public async Task<IActionResult> CreateSelectTime(int notificationId, [FromBody] TimeSpan time)
        {
            var result = await _technicianControlService.CreateselectTime(notificationId, time);

            if (!result)
                return BadRequest("Failed to create appointment time.");

            return Ok("Appointment time created successfully.");
        }

        [HttpPut("update-select-time/{notificationId}")]
        public async Task<IActionResult> UpdateSelectTime(int notificationId, [FromBody] TimeSpan? time)
        {
            var result = await _technicianControlService.UpdateSelectTime(notificationId, time);

            if (!result)
                return BadRequest("Failed to update appointment time.");

            return Ok("Appointment time updated successfully.");
        }

        [HttpPut("technician-on-the-way/{notificationId}")]
        public async Task<IActionResult> TechnicianOnTheWay(int notificationId)
        {
            var result = await _technicianControlService.TechnicianOnTheWay(notificationId);

            if (!result)
                return BadRequest("Failed to update technician status.");

            return Ok("Technician is on the way.");
        }

        [HttpPut("technician-arrived/{notificationId}")]
        public async Task<IActionResult> TechnicianArrived(int notificationId)
        {
            var result = await _technicianControlService.TechnicianArrived(notificationId);

            if (!result)
                return BadRequest("Failed to update technician status.");

            return Ok("Technician has arrived.");
        }

        [HttpPut("work-started/{notificationId}")]
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
    }
}

