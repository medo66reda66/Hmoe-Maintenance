using Ecommers.Api.Utilities;
using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hmoe_Maintenance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles =$"{DS.ADMIN_ROLE}")]
    public class AdminCompanyAndtechController : ControllerBase
    {
        private readonly IAdminCompanyTechService _adminCompanyService;

        public AdminCompanyAndtechController(IAdminCompanyTechService adminCompanyService)
        {
            _adminCompanyService = adminCompanyService;
        }

        [HttpPost("send-notification")]
        public async Task<IActionResult> sendNotification(CreateSendNotificationRequest request)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (adminId == null)
            {
                return Unauthorized();
            }
            var result = await _adminCompanyService.Sendnotification(adminId, request);
            if (result == null)
            {
                return BadRequest("Failed to send notification.");
            }
            return Ok("Notification sent successfully.");
        }

        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotification([FromQuery]FilternotificationRequest? filternotification,[FromQuery]int page =1)
        {
            var adminid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (adminid == null) 
            {
                return NotFound();
            }

            var Notificatins =await _adminCompanyService.GetNotification(adminid,filternotification,page);
            if (Notificatins.Datarequest == null) 
            {
                return BadRequest("Not fount Notificatins ");
            }

            return Ok(Notificatins);
        }

        [HttpGet("notifications/{notificationId}")]
        public async Task<IActionResult> GetNotificationById(int notificationId)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (adminId == null)
                return Unauthorized();

            var notification = await _adminCompanyService.GetNotificationBYid(adminId, notificationId);

            if (notification == null)
                return NotFound("Notification not found.");

            return Ok(notification);
        }

        [HttpGet("companies")]
        public async Task<IActionResult> GetAllCompany([FromQuery]FiltercompanyReqest filtercompany , int page = 1)
        {
            var companies = await _adminCompanyService.GetAllCompany(filtercompany,page);

            if (companies.Datarequest == null || !companies.Datarequest.Any())
                return NotFound("No companies found.");

            return Ok(companies);
        }

        [HttpGet("companies/{companyId}")]
        public async Task<IActionResult> GetCompanyById(int companyId)
        {
            var company = await _adminCompanyService.GetCompanyById(companyId);

            if (company == null)
                return NotFound("Company not found.");

            return Ok(company);
        }

        [HttpGet("coverage-areas")]
        public async Task<IActionResult> GetAllCompanyCoverageAreas([FromQuery] FiltercompanyReqest filtercompanyArea , int page = 1)
        {
            var coverageAreas = await _adminCompanyService.GetAllCompanyCoverageAreas(filtercompanyArea,page);

            if (coverageAreas.Datarequest == null || !coverageAreas.Datarequest.Any())
                return NotFound("No coverage areas found.");

            return Ok(coverageAreas);
        }

        [HttpGet("companies/{companyId}/coverage-areas")]
        public async Task<IActionResult> GetCompanyCoverageAreaById(int companyId)
        {
            var coverageAreas = await _adminCompanyService.GetCompanyCoverageAreaById(companyId);

            if (coverageAreas == null || !coverageAreas.Any())
                return NotFound("No coverage areas found.");

            return Ok(coverageAreas);
        }

        [HttpGet("GetAllTechnicianProfiles")]
        public async Task<IActionResult> GetAllTechnicianProfiles([FromQuery] FilterTechnicianRequest filterTechnician,int page = 1)
        {
            var technicians = await _adminCompanyService.GetAllTechnicianProfiles(filterTechnician,page);
            if (technicians.Datarequest == null || !technicians.Datarequest.Any())
                return NotFound("No technician profiles found.");
            return Ok(technicians);
        }

        [HttpGet("GetTechnicianProfileById/{id}")]
        public async Task<IActionResult> GetTechnicianProfileById(int id)
        {
            var technician = await _adminCompanyService.GetTechnicianProfileById(id);
            if (technician == null)
                return NotFound($"Technician profile with ID {id} not found.");
            return Ok(technician);
        }

        [HttpPut("ApproveCompanyCreate/{notificationId}")]
        public async Task<IActionResult> ApproveCompanyCreate(int notificationId)
        {
            var result = await _adminCompanyService.ApproveCompanyCreate(notificationId);

            if (!result)
                return NotFound("Notification or Company not found.");

            return Ok("Company approved successfully.");
        }

        [HttpPut("ApproveCompanyUpdate/{notificationId}")]
        public async Task<IActionResult> ApproveCompanyUpdate(int notificationId)
        {
            var result = await _adminCompanyService.ApproveCompanyUpdate(notificationId);

            if (!result)
                return NotFound("Notification or Company not found.");

            return Ok("Company update approved successfully.");
        }

        [HttpPut("RejectCompanyCreate/{notificationId}")]
        public async Task<IActionResult> RejectCompanyCreate(int notificationId)
        {
            var result = await _adminCompanyService.RejectCompanyCreate(notificationId);

            if (!result)
                return NotFound("Notification or Company not found.");

            return Ok("Company creation rejected successfully.");
        }

        [HttpPut("RejectCompanyUpdate/{notificationId}")]
        public async Task<IActionResult> RejectCompanyUpdate(int notificationId)
        {
            var result = await _adminCompanyService.RejectCompanyUpdate(notificationId);

            if (!result)
                return NotFound("Notification or Company not found.");

            return Ok("Company update rejected successfully.");
        }
    }
}

