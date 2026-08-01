using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.Models;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hmoe_Maintenance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicianServiceController : ControllerBase
    {
        private readonly ITechnicianerviceSesrvice _technicianService;

        public TechnicianServiceController(ITechnicianerviceSesrvice technicianervice)
        {
            _technicianService = technicianervice;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _technicianService.GetAllTechnicianService();

            return Ok(result);
        }

        // Get By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _technicianService.GetTechnicianServiceById(id);

            if (result == null)
                return NotFound("Technician Service Not Found");

            return Ok(result);
        }

        // Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TechnicianServiceRequest technicianService)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tecid = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _technicianService.createTechnicianervice(technicianService,tecid);

            if (result == null)
                return BadRequest("Failed to create Technician Service");

             return RedirectToAction(nameof(GetById), new { id = result.Id });
        }

        // Update
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TechnicianServiceRequest technicianService)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _technicianService.UpdateTechnicianervice(id, technicianService);

            if (result == null)
                return NotFound("Technician Service Not Found");

            return Ok(result);
        }

        // Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _technicianService.DeleteTechnicianervice(id, null);

            if (!result)
                return NotFound("Technician Service Not Found");

            return NoContent();
        }

    }
}
