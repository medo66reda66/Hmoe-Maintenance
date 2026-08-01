using System.ComponentModel.DataAnnotations;

namespace Hmoe_Maintenance.DTOs.Request
{
    public class UpdateCompanyRequest
    {
        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public string Description { get; set; } = default!;
        public IFormFile? LogoUrl { get; set; }
        [Required]
        public string PhoneNumber { get; set; } = default!;
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public IFormFile? LicenseImageUrl { get; set; }
        [Required]
        public string? CommercialRegistrationNumber { get; set; }
        [Required]
        public IFormFile? CommercialRegistrationImageUrl { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;

    }
}
