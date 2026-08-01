using Hmoe_Maintenance.Services;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hmoe_Maintenance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminTechnicianController : ControllerBase
    {
        private readonly IAdminTechnicianService _adminTechnicianService;

        public AdminTechnicianController(IAdminTechnicianService adminTechnicianService)
        {
            _adminTechnicianService = adminTechnicianService;
        }

        [HttpPost("ApproveTechnicianCreate/{notifId}")]
        public async Task<IActionResult> ApproveTechnicianCreate(int notifId)
        {
            var result = await _adminTechnicianService.ApproveTechnicienCreate(notifId);

            if (!result)
                return NotFound("Notification or Technician not found.");

            return Ok("Technician application approved successfully.");
        }

        [HttpPost("RejectTechnicianCreate/{notifId}")]
        public async Task<IActionResult> RejectTechnicianCreate(int notifId)
        {
            var result = await _adminTechnicianService.RejectTechnicienCreate(notifId);

            if (!result)
                return NotFound("Notification or Technician not found.");

            return Ok("Technician application rejected successfully.");
        }

        [HttpPost("ApproveTechnicianUpdate/{notifId}")]
        public async Task<IActionResult> ApproveTechnicianUpdate(int notifId)
        {
            var result = await _adminTechnicianService.ApproveTechnicianUpdate(notifId);

            if (!result)
                return NotFound("Notification or Update request not found.");

            return Ok("Technician profile update approved successfully.");
        }

        [HttpPost("RejectTechnicianUpdate/{notifId}")]
        public async Task<IActionResult> RejectTechnicianUpdate(int notifId)
        {
            var result = await _adminTechnicianService.RejectTechnicianUpdate(notifId);

            if (!result)
                return NotFound("Notification or Update request not found.");

            return Ok("Technician profile update rejected successfully.");
        }
    }
}

