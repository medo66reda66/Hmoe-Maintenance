using Hmoe_Maintenance.DTOs.Request;
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
    //[Authorize]
    public class CompanyServiceController : ControllerBase
    {
       private readonly ICompanyServiceservice _companyServiceservice;

       public CompanyServiceController(ICompanyServiceservice companyServiceservice)
       {
           _companyServiceservice = companyServiceservice;
       }

        [HttpGet("GetAllCompanyServices")]
        public async Task<IActionResult> GetAllCompanyServices()
        {
            var companyServices = await _companyServiceservice.GetAllCompanyServices();
            if (companyServices == null)
            {
                return NotFound("No company services found.");
            }
            return Ok(companyServices);
        }
        [HttpGet("GetCompanyServiceById/{id}")]
        public async Task<IActionResult> GetCompanyServiceById(int id)
        {
            var companyService = await _companyServiceservice.GetCompanyServiceById(id);
            if (companyService == null)
            {
                return NotFound($"Company service with ID {id} not found.");
            }
            return Ok(companyService);
        }
        [HttpPost("CreateCompanyService")]
        public async Task<IActionResult> CreateCompanyService(CreateCompanyServiceRequest createCompanyServiceRequest)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            if (createCompanyServiceRequest == null)
            {
                return BadRequest("Company service data is null.");
            }
            var createdCompanyService = await _companyServiceservice.CreateCompanyService(createCompanyServiceRequest, userId);
            if (createdCompanyService == null)
            {
                return BadRequest("Failed to create company service.");
            }
            return CreatedAtAction(nameof(GetCompanyServiceById), new { id = createdCompanyService.Id }, createdCompanyService);
        }
        [HttpPut("UpdateCompanyService/{id}")]
        public async Task<IActionResult> UpdateCompanyService(int id, UpdateCompanyServiceRequest updateCompanyServiceRequest)
        {
            if (updateCompanyServiceRequest == null)
            {
                return BadRequest("Company service data is null.");
            }
            var updatedCompanyService = await _companyServiceservice.UpdateCompanyService(id, updateCompanyServiceRequest);
            if (updatedCompanyService == null)
            {
                return BadRequest("Failed to update company service.");
            }
            return Ok(updatedCompanyService);
        }
        [HttpDelete("DeleteCompanyService/{id}")]
        public async Task<IActionResult> DeleteCompanyService(int id)
        {
            var deletedCompanyService = await _companyServiceservice.DeleteCompanyService(id);
            if (!deletedCompanyService)
            {
                return NotFound("Company service not found.");
            }
            return NoContent();
        }
    }
}
