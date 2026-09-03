using Hmoe_Maintenance.DataBase;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services
{
    public class ProfileUserService : Services.Interfaces.IProfileUserService 
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDBcontext _context;
        public ProfileUserService(UserManager<ApplicationUser> userManager, AppDBcontext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<ProfileUserResponse> GetProfileUserByIdAsync(string userId)
        {
            var profileUser = await _userManager.FindByIdAsync(userId);
            if (profileUser == null)
            {
                return null;
            }
            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.ApplicationUserId == userId);
            if (address == null)
            {
                return null;
            }
            var profileUserResponse = new ProfileUserResponse
            {
                Id = profileUser.Id,
                UserName = profileUser.UserName,
                Email = profileUser.Email,
                PhoneNumber = profileUser.PhoneNumber,
                FullName = profileUser.FullName,
                Title = address.Title,
                Governorate = address.Governorate,
                City = address.City,
                Area = address.Area,
                Street = address.Street,
                BuildingNumber = address.BuildingNumber,
                Floor = address.Floor,
                ApartmentNumber = address.ApartmentNumber
            };
            return profileUserResponse;
        }
        public async Task<bool> UpdateProfileUserAsync(ProfileUserResponse profileUser)
        {
            var existingUser = await _userManager.FindByIdAsync(profileUser.Id);
            if (existingUser == null)
            {
                return false;
            }
            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.ApplicationUserId == profileUser.Id);
            if (address == null)
            {
                return false;
            }
            existingUser.UserName = profileUser.UserName;
            existingUser.Email = profileUser.Email;
            existingUser.PhoneNumber = profileUser.PhoneNumber;
            existingUser.FullName = profileUser.FullName;
            address.Title = profileUser.Title;
            address.Governorate = profileUser.Governorate;
            address.City = profileUser.City;
            address.Area = profileUser.Area;
            address.Street = profileUser.Street;
            address.BuildingNumber = profileUser.BuildingNumber;
            address.Floor = profileUser.Floor;
            address.ApartmentNumber = profileUser.ApartmentNumber;

            await _userManager.UpdateAsync(existingUser);
            _context.Addresses.Update(address);

            return true;
        }
        public async Task<bool> UpdatePasswordAsync(ProfileUserResponse profileUser)
        {
            var user = await _userManager.FindByIdAsync(profileUser.Id);
            if (user == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(profileUser.CurrentPassword) || string.IsNullOrEmpty(profileUser.NewPassword))
            {
                return false;
            }

            var result = await _userManager.ChangePasswordAsync(user, profileUser.CurrentPassword, profileUser.NewPassword);
            if (!result.Succeeded)
            {
                return false;
            }

            return result.Succeeded;
        }
    }
}
