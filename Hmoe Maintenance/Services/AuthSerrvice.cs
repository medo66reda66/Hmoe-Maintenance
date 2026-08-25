using Azure.Core;
using Ecommers.Api.Utilities;
using Ecommers.Api.ViewModels;
using Hmoe_Maintenance.DataBase;
using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Request.Loging;
using Hmoe_Maintenance.Models;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Issuing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hmoe_Maintenance.Services
{
    public class AuthService : IAuthService
    {
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDBcontext _context;
        private readonly IEmailSender _emailSender;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<ApplicationUser> userManager, AppDBcontext context, IActionContextAccessor actionContextAccessor, IUrlHelperFactory urlHelper, IEmailSender emailSender, ITokenService tokenService, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _actionContextAccessor = actionContextAccessor;
            _urlHelperFactory = urlHelper;
            _emailSender = emailSender;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> Register(RegisterRequest registerRequest, AddressRequest addressRequest)
        {
            var user = new ApplicationUser
            {
                UserName = registerRequest.Email,
                Email = registerRequest.Email,
                FullName = registerRequest.FullName,
                PhoneNumber = registerRequest.PhoneNumber,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);
            
            if(!result.Succeeded)
            {
                return result;
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext).Action("ConfirmEmail", "Auth", new { userId = user.Id, token = token }, _actionContextAccessor.ActionContext.HttpContext.Request.Scheme);
            await _emailSender.SendEmailAsync(
                user.Email,
                "Confirm your email",
                $"Please confirm your account by clicking this link: <a href='{link}'>link</a>");

            var address = new Models.Address
            {
                ApplicationUserId = user.Id,
                Title = addressRequest.Title,
                Governorate = addressRequest.Governorate,
                City = addressRequest.City,
                Area = addressRequest.Area,
                Street = addressRequest.Street,
                BuildingNumber = addressRequest.BuildingNumber,
                Floor = addressRequest.Floor,
                ApartmentNumber = addressRequest.ApartmentNumber,
                Landmark = addressRequest.Landmark,
                Latitude = addressRequest.Latitude,
                CreatedAt = DateTime.UtcNow
            };

            _context.Addresses.Add(address);
            _context.SaveChanges();

            var UserRole = registerRequest.roles.ToString();
            if (UserRole==DS.COMPANYOWNER_ROLE)
            {
                await _userManager.AddToRoleAsync(user, DS.COMPANYOWNER_ROLE);
            }
            else if(UserRole == DS.TECHNICAL_ROLE)
            {
                await _userManager.AddToRoleAsync(user, DS.TECHNICAL_ROLE);
            }
            else
            {
                await _userManager.AddToRoleAsync(user, DS.CLIENT_ROLE);
            }

                return result;
        }

        public async Task<bool> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.ConfirmEmailAsync(user, token);

            return result.Succeeded;
        }
        public async Task<ForgetPasswordRequest> ForgetPassword(ForgetPasswordRequest forgetPassword)
        {
            var user =await _userManager.FindByEmailAsync(forgetPassword.UserNmaeEmail);
            if(user is null)
            {
                return null;
            }
            var last24Hours = DateTime.UtcNow.AddHours(-24);

            var userOtps = _context.applicationuserOtps.Where(e=>e.Applicationuserid == user.Id);

            var totalOtp = await userOtps.CountAsync(e => e.CreateAt >= last24Hours);

            if (totalOtp > 5)
            {
                return null;
            }

            var otp = new Random().Next(10000,99999).ToString();

            await _context.applicationuserOtps.AddAsync(new()
            {
                id = Guid.NewGuid().ToString(),
                Applicationuserid = user.Id,
                CreateAt = DateTime.UtcNow,
                Isvalid = true,
                Otp = otp,
                Validto = DateTime.UtcNow.AddDays(1),
            });
           await _context.SaveChangesAsync();

           await _emailSender.SendEmailAsync(user.Email!, "Houme Maintenance - Reset Your Password"
                , $"<h1>Use This OTP: {otp} To Reset Your Account. Don't share it.</h1>");
            return forgetPassword;

        }

        public async Task<ApplicationuserOtp> ValidateOTP(ValidateOTPRequest validateOTP)
        {
            var result =await _context.applicationuserOtps.FirstOrDefaultAsync(e => e.Applicationuserid == validateOTP.ApplicationUserId
            && e.Otp == validateOTP.OTP && e.Isvalid && e.Validto > DateTime.UtcNow);
    
            if(result == null)
            {
                return null;
            }
            result.Isvalid = false;
            await _context.SaveChangesAsync();

            return result;
        }
        public async Task<IdentityResult> NewPassword(NewPasswordRequest newPassword)
        {
            var user =await _userManager.FindByIdAsync(newPassword.ApplicationUserId);

            if(user == null) 
            {
                return null;
            }

            var token =await _userManager.GeneratePasswordResetTokenAsync(user);

            var result =await _userManager.ResetPasswordAsync(user, token,newPassword.Password);

            return result;
        }

        public  async Task<LogingResult> Login(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null) return new LogingResult { Status = LoginStatus.UserNotFound };

            var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, loginRequest.RememberMe, true);
           
                if (result.IsLockedOut)
                {
                    return new LogingResult { Status = LoginStatus.LockedOut };
                }
                else if (!user.EmailConfirmed)
                {
                    return new LogingResult { Status = LoginStatus.EmailNotConfirmed };
                }
                else if (!result.Succeeded)
                {
                    return new LogingResult { Status = LoginStatus.InvalidPassword };
                }
            

            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, userRoles.FirstOrDefault() ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = _tokenService.CreateToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiration = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(user);


            return new LogingResult { Status = LoginStatus.Success,
                RefreshToken = refreshToken,
                RefreshTokenTime="7 day", 
                Token = token, 
                Validtoken = "10 minutes" };
        }
        

    }

}

