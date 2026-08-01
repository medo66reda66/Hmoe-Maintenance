namespace Hmoe_Maintenance.DTOs.Request
{
    public class UpdateCompanyCoverageAreaRequest
    {
        public string Governorate { get; set; } = default!;
        public string City { get; set; } = default!;
        public string? Area { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
