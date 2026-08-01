using Hmoe_Maintenance.Models;
using System.ComponentModel.DataAnnotations;

namespace Hmoe_Maintenance.DTOs.Request
{
    public enum Roles
    {
        CompanyOwner,
        Technical,
        Client,
    }
    public class RegisterRequest
    {
        [Required]
        public string UserName { get; set; }=string.Empty;
        [Required]
        public string FullName { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required,DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }= string.Empty;
        [Required(ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public Roles roles {  get; set; }

    }
}
