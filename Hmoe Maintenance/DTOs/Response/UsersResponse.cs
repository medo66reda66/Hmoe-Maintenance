using Hmoe_Maintenance.Models;
using Microsoft.AspNetCore.Identity;

namespace Hmoe_Maintenance.DTOs.Response
{
    public class UsersResponse
    {
        public string id {  get; set; }
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        public bool? LockoutEnabled { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
}
