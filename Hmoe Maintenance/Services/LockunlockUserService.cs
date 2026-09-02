using Ecommers.Api.Utilities;
using Hmoe_Maintenance.DataBase;
using Hmoe_Maintenance.DTOs.Request;
using Hmoe_Maintenance.DTOs.Request.filter;
using Hmoe_Maintenance.DTOs.Response;
using Hmoe_Maintenance.DTOs.Response.filter;
using Hmoe_Maintenance.Models;
using Hmoe_Maintenance.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hmoe_Maintenance.Services
{
    public class LockunlockUserService : ILockunlockUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDBcontext _dBcontext;

        public LockunlockUserService(UserManager<ApplicationUser> userManager, AppDBcontext dBcontext)
        {
            _userManager = userManager;
            _dBcontext = dBcontext;
        }

        public async Task<PaginationResponse<UsersResponse,FilterUsersResponse>> Index(FilterUsersRequest filter,int page)
        {
            var users = _userManager.Users.AsQueryable()
                .Select(users => new UsersResponse
                {
                    id = users.Id,
                    Email = users.Email,
                    Fullname = users.FullName,
                    LockoutEnabled = users.LockoutEnabled,
                    CreatedAt = users.CreatedAt,
                });
            

            if (users == null) return null;

            FilterUsersResponse filterUsersResponse = new();
            if(filter.id != null)
            {
                users = users.Where(e=>e.id == filter.id);
                filterUsersResponse.Id = filter.id;
            }
            if(filter.Email  != null)
            {
                users = users.Where(e=>e.Email == filter.Email);
                filterUsersResponse.Email = filter.Email;
            }
            if(filter.Fullname != null)
            {
                users = users.Where(e=>e.Fullname.Contains(filter.Fullname));
                filterUsersResponse.Fullname = filter.Fullname;
            }
            if(filter.loukout.HasValue)
            {
                users = users.Where(e=>e.LockoutEnabled == filter.loukout.Value);
                filterUsersResponse.Lockout = filter.loukout.Value;
            }

            var result =await PaginationService.PaginateAsync(users, page, filterUsersResponse, 10);
            return result;

        }
        public async Task<bool> Loukunlouk(string userid)
        {
            var user = await _userManager.FindByIdAsync(userid);
            if (user == null) return false;

            if(await _userManager.IsInRoleAsync(user,DS.ADMIN_ROLE))
            {
                return false;
            }

            user.LockoutEnabled = !user.LockoutEnabled;

            if (!user.LockoutEnabled)
            {
                user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(30);
            }
            else
            {
                user.LockoutEnd = null;
            }
             
           await _userManager.UpdateAsync(user);

            return true;
        }
    }
}
