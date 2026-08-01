using System.ComponentModel.DataAnnotations;

namespace Hmoe_Maintenance.DTOs.Request.Loging
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        public bool RememberMe { get; set; } = false;

    }
}
