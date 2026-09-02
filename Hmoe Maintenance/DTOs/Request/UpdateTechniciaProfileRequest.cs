using System.ComponentModel.DataAnnotations;

namespace Hmoe_Maintenance.DTOs.Request
{
    public class UpdateTechniciaProfileRequest
    {
        public int CompanycopyId { get; set; }
        [Required]
        public string Fullname { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PhoneNumper { get; set; }
        [Required]
        public string NationalId { get; set; } = default!;
        [Required]
        public IFormFile? ProfileImageUrl { get; set; } = default!;
        [Required]
        public IFormFile? NationalIdFrontImageUrl { get; set; } = default!;
        [Required]
        public IFormFile? NationalIdBackImageUrl { get; set; } = default!;
        [Required]
        public IFormFile? TechnicianDocumentUrl { get; set; } = default!;
        [Required]
        public int YearsOfExperience { get; set; }
        public string? Bio { get; set; }
        public bool IsAvailable { get; set; } = true;
        public bool IsActive { get; set; } = true;

    }
}
