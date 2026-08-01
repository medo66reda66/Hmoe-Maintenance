using Ecommers.Api.ViewModels;
using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Request.Loging;
using Hmoe_Maintenance.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> Register(RegisterRequest registerRequest, AddressRequest addressRequest);
        Task<bool> ConfirmEmail(string userId, string token);
        Task<LogingResult> Login(LoginRequest loginRequest);
        Task<ForgetPasswordRequest> ForgetPassword(ForgetPasswordRequest forgetPassword);
        Task<ApplicationuserOtp> ValidateOTP(ValidateOTPRequest validateOTP);
        Task<IdentityResult> NewPassword(NewPasswordRequest newPassword);
      

     }
}
