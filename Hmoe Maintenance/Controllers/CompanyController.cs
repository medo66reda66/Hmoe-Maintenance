using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.Models;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hmoe_Maintenance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly ICompanyControlService _companyControlService;
        public CompanyController(ICompanyService companyService, ICompanyControlService companyControlService)
        {
            _companyService = companyService;
            _companyControlService = companyControlService;
        }

        [HttpGet("GetAllCompanies")]
        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await _companyService.GetAllCompany();
            if (companies == null) {
                return NotFound(new { message = "No companies found" });
            }

            return Ok(companies);
        }

        [HttpGet("GetCompanyById/{id}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            var company = await _companyService.GetCompanyById(id);
            if (company == null)
            {
                return NotFound(new { message = "Company not found" });
            }
            return Ok(company);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCompany(CreateCompanyRequest companyRequest)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null)
            {
                return Unauthorized(new { message = "User is not authenticated" });
            }
            var company = await _companyService.CreateCompany(companyRequest, userid);
            if (company == null)
            {
                return BadRequest(new { message = "Failed to create company" });
            }
            return CreatedAtAction("GetCompanyById", new { id = company.Id }, company);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCompany(int id, UpdateCompanyRequest updateCompanyRequest)
        {
            var company = await _companyService.UpdateCompany(id, updateCompanyRequest);
            if (company == null)
            {
                return NotFound(new { message = "Company not found" });
            }
            return Ok(new { message = "Company updated successfully", company });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var result = await _companyService.DeleteCompany(id);

            if (!result)
            {
                return NotFound(new { message = "Company not found" });
            }
            return NoContent();
        }

        [HttpGet("GetAllNotification")]
        public async Task<IActionResult> GetAllNotification()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null)
            {
                return BadRequest("user not found");
            }
            var allnotifications =await _companyControlService.GetAllNotificationToCompany(userid);
            if (allnotifications == null)
            {
                return BadRequest("No Notificcatin");
            }

            return Ok(allnotifications);
        }
        [HttpGet("GetAllNotificationByid/{Notid}")]
        public async Task<IActionResult> GetAllNotificationByid(int Notid)
        {
            var comid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (comid == null)
            {
                return BadRequest("user not found");
            }
            var allnotification =await _companyControlService.GetAllNotificationBycompanyById(Notid, comid);
            if (allnotification == null)
            {
                return BadRequest("No Notificcatin");
            }

            return Ok(allnotification);
        }


        [HttpPost("ApproveMaintenanceRequest/{Notid}")]
        public async Task<IActionResult> ApproveMaintenanceRequest(int Notid)
        {
            var approverequest =await _companyControlService.ApprovecompanyRequest(Notid);
            if (approverequest == null)
            {
                return BadRequest();
            }
            return Ok("Company Approved Request");
        }

        [HttpPost("RejectedMaintenanceRequest/{Notid}")]
        public async Task<IActionResult> RejectedMaintenanceRequest(int Notid)
        {
            var rejectedrequest =await _companyControlService.RejectCompanyRequest(Notid);
            if (rejectedrequest == null)
            {
                return BadRequest();
            }
            return Ok("Company Approved Request");
        }

        [HttpPost("CreatepriseReuest")]
        public async Task<IActionResult> CreatepriseReuest(int notid,CreatepriceRequest createprice)
        {
            var pricereq =await _companyControlService.Createprisebycompany(notid, createprice);
            if(pricereq == null)
            {
                return BadRequest();
            }

            return Ok(pricereq);
        }

        [HttpPost("AssignedTechnicianRequest/{Notid}")]
        public async Task<IActionResult> AssignedTechnicianRequest(int Notid,[FromBody]int tecuserid)
        {
            var result =await _companyControlService.AssignedTechnicianRequest(Notid, tecuserid);
            if (result == false)
            {
                return BadRequest();
            }

            return Ok();
        }

    }
}


