using Ecommers.Api.Utilities;
using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.Models;
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
    [Authorize(Roles =$"{DS.COMPANYOWNER_ROLE}")]
    public class AdminTechnicianBycompController : ControllerBase
    {
        private readonly Services.Interfaces.IAdminTechnicianByCOMPService _adminTechnicianService;

        public AdminTechnicianBycompController(Services.Interfaces.IAdminTechnicianByCOMPService adminTechnicianService)
        {
            _adminTechnicianService = adminTechnicianService;
        }

        [HttpGet("GetAllTechnicianProfiles")]
        public async Task<IActionResult> GetAllTechnicianProfiles([FromQuery]FilterTechnicianRequest technicianRequest,int page = 1)
        {
            var compid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (compid == null)
            {
                return BadRequest("user not found");
            }
            var technicians = await _adminTechnicianService.GetAllTechnicianProfiles(compid, technicianRequest , page);
            if (technicians.Datarequest == null || !technicians.Datarequest.Any())
            {
                return NotFound("No technician profiles found.");
            }
            return Ok(technicians);
        }

        [HttpGet("GetTechnicianProfilesBYid/{id}")]
        public async Task<IActionResult> GetTechnicianProfilesBYid(int id)
        {
            var compid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (compid == null)
            {
                return BadRequest("user not found");
            }
            var technician = await _adminTechnicianService.GetTechnicianProfilesBYid(compid ,id);
            if (technician == null)
            {
                return NotFound($"Technician profile with ID {id} not found.");
            }
            return Ok(technician);
        }

        [HttpGet("GetTechPayout")]
        public async Task<IActionResult> GetTechPayout([FromQuery] FilterTechPayoutRequest filter,int page = 1)
        {
                var compid = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (compid == null)
                {
                     return Unauthorized("User not found");
                }

                var result = await _adminTechnicianService.GetTechpayout(compid,filter,page);

                if (result.Datarequest == null)
                {
                     return NotFound("No technician payouts found");
                }

             return Ok(result);
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

        [HttpPut("Loukunloukteck/{id}")]
        public async Task<IActionResult> Loukunloukteck(int id)
        {
            var result =await _adminTechnicianService.LockUnlockTech(id);
            if (!result)
                return NotFound();

            return RedirectToAction("GetAllTechnicianProfiles", "AdminTechnicianBycomp");
        }

        [HttpPost("TechnicianPayout/{nationalId}")]
        public async Task<IActionResult> TechnicianPayout(string nationalId,string notes)
        {
            var payout = await _adminTechnicianService.TechnicianPayout(
                nationalId,
                notes);

            if (payout == null)
            {
                return NotFound("Technician not found.");
            }

            return Ok(payout);
        }

    }
}

