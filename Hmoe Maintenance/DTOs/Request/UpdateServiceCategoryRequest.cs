namespace Hmoe_Maintenance.DTOs.Request
{
    public class UpdateServiceCategoryRequest
    {
        public string? Name { get; set; } = default!;
        public string? Description { get; set; } = default!;
        public IFormFile? IconUrl { get; set; }
        public bool? IsActive { get; set; } = true;

        public decimal? CompanyServiceInspectionPrice { get; set; } = default!;

        public decimal? CompanyServiceStartingPrice { get; set; } = default!;

        public string? CompanyServicecoDescription { get; set; } = default!;

        public bool? CompanyServiceIsActive { get; set; } = true;
    }
}
