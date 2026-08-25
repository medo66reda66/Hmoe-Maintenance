namespace Hmoe_Maintenance.DTOs.Response.filter
{
    public class FilterTechnicianResponse
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? NationalId { get; set; }
        public string? CompanyName { get; set; }
        public bool? IsAvailable { get; set; }
        public bool? IsActive { get; set; }
        public string? TechnicalService { get; set; }
    }
}
