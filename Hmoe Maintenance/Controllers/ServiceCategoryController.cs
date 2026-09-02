using Ecommers.Api.Utilities;
using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hmoe_Maintenance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.COMPANYOWNER_ROLE}"))]
    public class ServiceCategoryController : ControllerBase
    {
        private readonly IServiceCCategory _serviceCategory;
        public ServiceCategoryController(IServiceCCategory serviceCategory)
        {
            _serviceCategory = serviceCategory;
        }

        [HttpGet("GetServiceCategory")]
        public async Task<IActionResult> GetServiceCategory()
        {
            var services = await _serviceCategory.GetServiceCategory();

            if (services == null)
            {
                return NotFound("No service categories found.");
            }

            return Ok(services);
        }

        [HttpPost("Create")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.COMPANYOWNER_ROLE}"))]
        public async Task<IActionResult> CreateServiceCategory(CreateServiceCategoryRequest createServiceCategoryRequest)
        {
            var comid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (comid == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            var createdServiceCategory = await _serviceCategory.CreateServiceCategory(comid,createServiceCategoryRequest);
            if (createdServiceCategory == null)
            {
                return BadRequest("Failed to create service category.");
            }
            return Ok(createdServiceCategory);
        }
        [HttpPut("Update/{id}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.COMPANYOWNER_ROLE}"))]
        public async Task<IActionResult> UpdateServiceCategory(int id, UpdateServiceCategoryRequest updateServiceCategoryRequest)
        {
            var updatedServiceCategory = await _serviceCategory.UpdateServiceCategory(id, updateServiceCategoryRequest);
            if (updatedServiceCategory == null)
            {
                return BadRequest("Failed to update service category.");
            }
            return Ok(updatedServiceCategory);
        }
        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = ($"{DS.ADMIN_ROLE},{DS.COMPANYOWNER_ROLE}"))]
        public async Task<IActionResult> DeleteServiceCategory(int id)
        {
            var deletedServiceCategory = await _serviceCategory.DeleteServiceCategory(id);

            if (!deletedServiceCategory)
            {
                return NotFound("Service category not found.");
            }

            return NoContent();
        }


    }
}
