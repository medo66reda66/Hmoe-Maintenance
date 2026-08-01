using System.ComponentModel.DataAnnotations;

namespace Ecommers.Api.ViewModels
{
    public class ValidateOTPRequest
    {
      

        [Required]
        public string OTP { get; set; } = string.Empty;

        public string ApplicationUserId { get; set; } = string.Empty;
    }
}
