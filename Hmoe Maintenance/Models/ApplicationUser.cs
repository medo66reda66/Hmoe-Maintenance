using Microsoft.AspNetCore.Identity;

namespace Hmoe_Maintenance.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = default!;

        public string? ProfileImageUrl { get; set; }

        public List<Address>? DefaultAddress { get; set; } = new List<Address>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? RefreshToken { get; set; } = string.Empty;
        public DateTime? RefreshTokenExpiration { get; set; }
    }
}
