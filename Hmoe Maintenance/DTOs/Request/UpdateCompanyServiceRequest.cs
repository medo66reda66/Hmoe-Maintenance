namespace Hmoe_Maintenance.DTOs.Request
{
    public class UpdateCompanyServiceRequest
    {
        public int ServiceCategoryId { get; set; }
        public decimal InspectionPrice { get; set; }
        public decimal? StartingPrice { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
