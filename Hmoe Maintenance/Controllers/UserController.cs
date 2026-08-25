using Ecommers.Api.Utilities;
using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hmoe_Maintenance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles =DS.ADMIN_ROLE)]
    public class UserController : ControllerBase
    {
        private readonly ILockunlockUserService _lockunlockUserService;

        public UserController(ILockunlockUserService lockunlockUserService)
        {
            _lockunlockUserService = lockunlockUserService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery] FilterUsersRequest filter,
            int page = 1)
        {
            var result = await _lockunlockUserService.Index(filter, page);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("lock-unlock/{userid}")]
        public async Task<IActionResult> LockUnLock(string userid)
        {
            var result = await _lockunlockUserService.Loukunlouk(userid);

            if (!result)
                return BadRequest("User not found or cannot be locked/unlocked.");

            return Ok("Usr statuse update successfully");
        }
    }
}
