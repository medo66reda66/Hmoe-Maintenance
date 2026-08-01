using Ecommers.Api.Utilities;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hmoe_Maintenance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles =$"{DS.ADMIN_ROLE}")]
    public class AdminCompanyController : ControllerBase
    {
        private readonly IAdminCompanyService _adminCompanyService;

        public AdminCompanyController(IAdminCompanyService adminCompanyService)
        {
            _adminCompanyService = adminCompanyService;
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

