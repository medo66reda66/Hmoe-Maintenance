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
    [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.COMPANYOWNER_ROLE}"))]
    public class CompanyServiceController : ControllerBase
    {
       private readonly ICompanyServiceservice _companyServiceservice;

       public CompanyServiceController(ICompanyServiceservice companyServiceservice)
       {
           _companyServiceservice = companyServiceservice;
       }
       
        [HttpGet("GetMyCompanyService")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.COMPANYOWNER_ROLE}"))]
        public async Task<IActionResult> GetMyCompanyService()
        {
            var compid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(compid == null)
            {
                return BadRequest("company not found");
            }

            var companyService = await _companyServiceservice.GetMYCompanyServiceById(compid);
            if (companyService == null)
            {
                return NotFound($"Company service with not found.");
            }
            return Ok(companyService);
        }
        [HttpGet("GetAllCompanyServices")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.COMPANYOWNER_ROLE}"))]
        public async Task<IActionResult> GetAllCompanyServices([FromQuery]FiltercompanyserviceResquest filtercompanyservice,int page = 1)
        {
            var companyServices = await _companyServiceservice.GetAllCompanyServices(filtercompanyservice,page);
            if (companyServices.Datarequest == null)
            {
                return NotFound("No company services found.");
            }
            return Ok(companyServices);
        }

        [HttpGet("GetCompanyServiceById/{id}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.COMPANYOWNER_ROLE}"))]
        public async Task<IActionResult> GetoneCompanyServicesBYid(int id)
        {
            var companyService = await _companyServiceservice.GetoneCompanyServicesBYid(id);
            if (companyService == null)
            {
                return NotFound($"Company service with id {id} not found.");
            }
            return Ok(companyService);
        }

        [HttpPost("CreateServiceTomyCompany")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.COMPANYOWNER_ROLE}"))]
        public async Task<IActionResult> CreateServiceTomyCompany([FromBody] CreateCompanyServiceRequest createCompanyService)
        {
            var compid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (compid == null)
            {
                return NotFound();
            }

            var result = await _companyServiceservice.CreateServiceTomyCompany(
                compid,
                createCompanyService);

            if (result == null)
            {
                return NotFound("Company not found.");
            }

            return Ok(result);
        }
    }
}
