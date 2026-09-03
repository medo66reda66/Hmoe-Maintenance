using Hmoe_Maintenance.DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hmoe_Maintenance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileUserController : ControllerBase
    {
        private readonly Services.Interfaces.IProfileUserService _profileUserService;

        public ProfileUserController(Services.Interfaces.IProfileUserService profileUserService)
        {
            _profileUserService = profileUserService;
        }

        [HttpGet("GetProfileUser")]
        public async Task<IActionResult> GetProfileUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest("User not found.");
            }
            var profileUser = await _profileUserService.GetProfileUserByIdAsync(userId);
            if (profileUser == null)
            {
                return NotFound("Profile user not found.");
            }
            return Ok(profileUser);
        }
        [HttpPut("UpdateProfileUser")]
        public async Task<IActionResult> UpdateProfileUser([FromBody] ProfileUserResponse profileUser)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || userId != profileUser.Id)
            {
                return BadRequest("Invalid user ID.");
            }
            var result = await _profileUserService.UpdateProfileUserAsync(profileUser);
            if (!result)
            {
                return NotFound("Profile user not found or update failed.");
            }
            return NoContent();
        }
        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] ProfileUserResponse profileUser)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || userId != profileUser.Id)
            {
                return BadRequest("Invalid user ID.");
            }
            var result = await _profileUserService.UpdatePasswordAsync(profileUser);
            if (!result)
            {
                return NotFound("Profile user not found or password update failed.");
            }
            return NoContent();
        }

    }
}
