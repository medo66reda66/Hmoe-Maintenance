using Ecommers.Api.ViewModels;
using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Request.Loging;
using Hmoe_Maintenance.Models;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Threading.Tasks;

namespace Hmoe_Maintenance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthController(IAuthService authService, ITokenService tokenService, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _tokenService = tokenService;
            _userManager = userManager;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest,[FromQuery] AddressRequest addressRequest)
        {
            var result = await _authService.Register(registerRequest, addressRequest);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = "User registration failed.",
                    errors = result.Errors
                });
            }
            else
            {
                return Ok(new { message = "User registered successfully." });
            }
        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _authService.ConfirmEmail(userId, token);
            if (!result)
            {
                return BadRequest(new
                {
                    message = "Email confirmation failed."
                });
            }
            else
            {
                return Ok(new { message = "Email confirmed successfully." });
            }
        }
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
        {
            var result = await _authService.ForgetPassword(request);

            if (result == null)
            {
                return BadRequest("Invalid email or OTP request limit exceeded.");
            }

            return Ok(new
            {
                Message = "OTP has been sent to your email."
            });
        }
        [HttpPost("ValidateOTP")]
        public async Task<IActionResult> ValidateOTP([FromBody] ValidateOTPRequest validateOTP)
        {
            var result =await _authService.ValidateOTP(validateOTP);
            if(result == null)
            {
                return CreatedAtAction(nameof(ValidateOTP), new { userId = validateOTP.ApplicationUserId }, new
                {
                    Message = "Invalid OTP"
                });
            }
            return CreatedAtAction(nameof(NewPassword), new { userId = validateOTP.ApplicationUserId }, new
            {
                Message = "OTP validated successfully. You can now reset your password."
            });
        }
        [HttpPost("NewPassword")]
        public async Task<IActionResult> NewPassword([FromBody] NewPasswordRequest newPassword)
        {
            var result =await _authService.NewPassword(newPassword);
            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    Message = "Password reset failed",
                    result.Errors
                });
            }

            return Ok(new
            {
                Message = "Password reset successfully"
            });
        }
        

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]LoginRequest loginRequest)
        {
            var result = await _authService.Login(loginRequest);

            switch (result.Status)
            {
                case LoginStatus.UserNotFound:
                    return BadRequest(new
                    {
                        message = "Login failed.",
                        errors = new[] { "User not found." }
                    });
                case LoginStatus.InvalidPassword:
                    return BadRequest(new
                    {
                        message = "Login failed.",
                        errors = new[] { "Invalid password." }
                    });
                case LoginStatus.EmailNotConfirmed:
                    return BadRequest(new
                    {
                        message = "Login failed.",
                        errors = new[] { "Email not confirmed." }
                    });
                case LoginStatus.LockedOut:
                    return BadRequest(new
                    {
                        message = "Login failed.",
                        errors = new[] { "User account is locked out." }
                    });
            }

            return Ok(new
            {
                message = "Login successful.",
                token = result.Token,
                refreshtoken = result.RefreshToken,
                refreshtokenTime = result.RefreshTokenTime,
                validToken = result.Validtoken
            });
        }
        [HttpPost, Authorize]
        [Route("Refresh")]
        public async Task<IActionResult> Refresh(TokenApiModel tokenApiModel)
        {
            if (tokenApiModel is null)
                return BadRequest("Invalid client request");
            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

            var username = principal.Identity.Name; 
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiration <= DateTime.Now)
                return BadRequest("Invalid client request");

            var newAccessToken = _tokenService.CreateToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                TOKEN = newAccessToken,
                Validto = "5 minutes",
                RefreshToken = newRefreshToken,
                Message = "Login successful."
            });
        }
        [HttpPost, Authorize]
        [Route("revoke")]
        public async Task<IActionResult> Revoke()
        {
            var username = User.Identity.Name;
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null) return BadRequest();
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            return NoContent();
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();

            return Ok("Logged out successfully");
        }

    }
}
