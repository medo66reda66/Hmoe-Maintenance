using System.ComponentModel.DataAnnotations;

namespace Hmoe_Maintenance.DTOs.Response
{
    public class CreateServiceCategoryRequest
    {
        [Required]
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public IFormFile? IconUrl { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
