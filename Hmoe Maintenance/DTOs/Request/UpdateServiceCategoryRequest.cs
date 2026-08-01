namespace Hmoe_Maintenance.DTOs.Request
{
    public class UpdateServiceCategoryRequest
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public IFormFile? IconUrl { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
