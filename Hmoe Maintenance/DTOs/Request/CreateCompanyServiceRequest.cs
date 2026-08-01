using System.ComponentModel.DataAnnotations;

namespace Hmoe_Maintenance.DTOs.Request
{
    public class CreateCompanyServiceRequest
    {
        [Required]
        public int ServiceCategoryId { get; set; }
        [Required]
        public decimal InspectionPrice { get; set; }
        [Required]
        public decimal? StartingPrice { get; set; }
        [Required]
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
