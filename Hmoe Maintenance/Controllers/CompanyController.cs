using Ecommers.Api.Utilities;
using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Request.filter;
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
    [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.COMPANYOWNER_ROLE}"))]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly ICompanyControlService _companyControlService;

        public CompanyController(ICompanyService companyService, ICompanyControlService companyControlService)
        {
            _companyService = companyService;
            _companyControlService = companyControlService;
        }

        [HttpGet("GetAllNotification")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.COMPANYOWNER_ROLE}"))]
        public async Task<IActionResult> GetAllNotification([FromQuery]FilternotificationRequest filternotification ,int page = 1)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null)
            {
                return BadRequest("user not found");
            }
            var allnotifications = await _companyControlService.GetAllNotificationToCompany(userid,filternotification,page);
            if (allnotifications.Datarequest == null)
            {
                return BadRequest("No Notificcatin");
            }

            return Ok(allnotifications);
        }

        [HttpGet("GetAllNotificationByid/{Notid}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.COMPANYOWNER_ROLE}"))]
        public async Task<IActionResult> GetAllNotificationByid(int Notid)
        {
            var comid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (comid == null)
            {
                return BadRequest("user not found");
            }
            var allnotification = await _companyControlService.GetAllNotificationBycompanyById(Notid, comid);
            if (allnotification == null)
            {
                return BadRequest("No Notificcatin");
            }

            return Ok(allnotification);
        }

        [HttpGet("GetMyCompany")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.COMPANYOWNER_ROLE}"))]
        public async Task<IActionResult> GetmyCompany()
        {
            var companyId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (companyId == null)
            {
                return Unauthorized(new { message = "User is not authenticated" });
            }
            var company = await _companyService.GetmyCompany(companyId);
            if (company == null)
            {
                return NotFound(new { message = "Company not found" });
            }
            return Ok(company);
        }

        [HttpPost("create")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.COMPANYOWNER_ROLE}"))]
        public async Task<IActionResult> CreateCompany(CreateCompanyRequest companyRequest,[FromQuery]CreateCompanyCoverageAreaRequest createCompanyCoverageArea)
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
           
            return Ok(new { message = "Company Create successfully", company });
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.COMPANYOWNER_ROLE}"))]
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
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.COMPANYOWNER_ROLE}"))]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var result = await _companyService.DeleteCompany(id);

            if (!result)
            {
                return NotFound(new { message = "Company not found" });
            }
            return NoContent();
        }
       
        [HttpGet("getAllclientPayment")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.COMPANYOWNER_ROLE}"))]
        public async Task<IActionResult> getAllclientPayment([FromQuery]FilterclientRequest filterclient,int page=1)
        {
            var compid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (compid == null)
            {
                return NotFound();
            }

            var payment =await _companyControlService.GetAllPaymentbyClient(compid,filterclient,page);
            if (payment.Datarequest == null)
            {
                return BadRequest("No Payment");
            }

            return Ok(payment);
        }


        [HttpPost("ApproveMaintenanceRequest/{Notid}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.COMPANYOWNER_ROLE}"))]
        public async Task<IActionResult> ApproveMaintenanceRequest(int Notid)
        {
            var approverequest =await _companyControlService.ApprovecompanyRequest(Notid);
            if (approverequest == false)
            {
                return BadRequest();
            }
            return Ok("Company Approved Request");
        }

        [HttpPost("RejectedMaintenanceRequest/{Notid}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.COMPANYOWNER_ROLE}"))]
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
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.COMPANYOWNER_ROLE}"))]
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
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.COMPANYOWNER_ROLE}"))]
        public async Task<IActionResult> AssignedTechnicianRequest(int Notid,int tecuserid)
        {
            var result =await _companyControlService.AssignedTechnicianRequest(Notid, tecuserid);
            if (result == false)
            {
                return BadRequest("Tech is not free");
            }

            return Ok();
        }

    }
}


