using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hmoe_Maintenance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ServiceCategoryController : ControllerBase
    {
        private readonly IServiceCCategory _serviceCategory;
        public ServiceCategoryController(IServiceCCategory serviceCategory)
        {
            _serviceCategory = serviceCategory;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServiceCategories()
        {
          var ServiceCategory = await _serviceCategory.GetAllServiceCategories();
            if (ServiceCategory == null)
            {
                return NotFound();
            }

            return Ok(ServiceCategory);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceCategoryById(int id)
        {
            var ServiceCategory = await _serviceCategory.GetServiceCategoryById(id);
            if (ServiceCategory == null)
            {
                return NotFound();
            }
            return Ok(ServiceCategory);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateServiceCategory(CreateServiceCategoryRequest createServiceCategoryRequest)
        {
            var createdServiceCategory = await _serviceCategory.CreateServiceCategory(createServiceCategoryRequest);
            if (createdServiceCategory == null)
            {
                return BadRequest("Failed to create service category.");
            }
            return CreatedAtAction(nameof(GetServiceCategoryById), new { id = createdServiceCategory.Id }, createdServiceCategory);
        }
        [HttpPut("Update/{id}")]
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
