using Ecommers.Api.Utilities;
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
    [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.TECHNICAL_ROLE}"))]
    public class TechnicianProfileController : ControllerBase
    {
        private readonly ITechnicianProfileServices _technicianProfileService;
        public TechnicianProfileController(ITechnicianProfileServices technicianProfileService)
        {
            _technicianProfileService = technicianProfileService;
        }

        [HttpGet("GetMyTechnicianById/{id}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.TECHNICAL_ROLE}"))]
        public async Task<IActionResult> GetMTechnicianById()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(new { message = "User is not authenticated" });
            }
            var technician = await _technicianProfileService.GetMyTechnicianProfile(userId);
            if (technician == null)
            {
                return NotFound(new { message = "Technician profile not found" });
            }
            return Ok(technician);
        }
        [HttpPost("create")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.TECHNICAL_ROLE}"))]
        public async Task<IActionResult> CreateTechnician( CreateTechnicianProfileRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(new { message = "User is not authenticated" });
            }
            var technician = await _technicianProfileService.CreateTechniciaProfile(request, userId);
            if (technician == null)
            {
                return BadRequest(new { message = "Failed to create technician profile" });
            }
            return Ok(technician);
        }
        [HttpPut("update/{id}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.TECHNICAL_ROLE}"))]
        public async Task<IActionResult> UpdateTechnician(int id, [FromForm] UpdateTechniciaProfileRequest request)
        {
            var technician = await _technicianProfileService.UpdateTechniciaProfile(id, request);
            if (technician == null)
            {
                return NotFound(new { message = "Technician profile not found" });
            }
            return Ok(new { message = "Technician profile updated successfully", technician });
        }
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.TECHNICAL_ROLE}"))]
        public async Task<IActionResult> DeleteTechnician(int id)
        {
            var result = await _technicianProfileService.DeleteTechnicianProfile(id);
            if (!result)
            {
                return NotFound(new { message = "Technician profile not found" });
            }
            return NoContent();
        }
    }
}