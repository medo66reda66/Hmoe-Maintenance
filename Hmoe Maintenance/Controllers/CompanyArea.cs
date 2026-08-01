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
    [Authorize]
    public class CompanyArea : ControllerBase
    {
        private readonly ICompanyCoverageAreaService _companyCoverageAreaService;

        public CompanyArea(ICompanyCoverageAreaService companyCoverageAreaService)
        {
            _companyCoverageAreaService = companyCoverageAreaService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllCompanyCoverageAreas()
        {
            var coverageAreas = await _companyCoverageAreaService.GetAllCompanyCoverageAreas();
            if (coverageAreas == null)
            {
                return NotFound("No company coverage areas found.");
            }
            return Ok(coverageAreas);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetCompanyCoverageAreaById(int id)
        {
            var coverageArea = await _companyCoverageAreaService.GetCompanyCoverageAreaById(id);
            if (coverageArea == null)
            {
                return NotFound($"Company coverage area with ID {id} not found.");
            }
            return Ok(coverageArea);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateCompanyCoverageArea(CreateCompanyCoverageAreaRequest request)
        {
            if (request == null)
            {
                return BadRequest("Company coverage area data is null.");
            }
            // Assuming you have a way to get the userId, for example from the authenticated user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("User is not authenticated.");
            }
            var createdCoverageArea = await _companyCoverageAreaService.CreateCompanyCoverageArea(request, userId);
            if (createdCoverageArea == null)
            {
                return BadRequest("Failed to create company coverage area.");
            }
            return CreatedAtAction(nameof(GetCompanyCoverageAreaById), new { id = createdCoverageArea.Id }, createdCoverageArea);
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateCompanyCoverageArea(int id, UpdateCompanyCoverageAreaRequest request)
        {
            if (request == null)
            {
                return BadRequest("Company coverage area data is null.");
            }
            var updatedCoverageArea = await _companyCoverageAreaService.UpdateCompanyCoverageArea(id, request);
            if (updatedCoverageArea == null)
            {
                return NotFound($"Company coverage area with ID {id} not found.");
            }
            return Ok(updatedCoverageArea);
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteCompanyCoverageArea(int id)
        {
            var deletedCoverageArea = await _companyCoverageAreaService.DeleteCompanyCoverageArea(id);
            if (deletedCoverageArea == null)
            {
                return NotFound($"Company coverage area with ID {id} not found.");
            }
            return NoContent();

        }
    }

}
