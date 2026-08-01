using System.ComponentModel.DataAnnotations;

namespace Hmoe_Maintenance.DTOs.Request
{
    public class ForgetPasswordRequest
    {
        [Required, EmailAddress]
        public string UserNmaeEmail { get; set; } = string.Empty;

    }
}
