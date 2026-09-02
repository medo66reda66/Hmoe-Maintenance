namespace Hmoe_Maintenance.DTOs.Response.filter
{
    public class FilterTechPayoutResponse
    {
        public string? Name { get; set; }
        public string? NationalId { get; set; }
        public string? Email { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
