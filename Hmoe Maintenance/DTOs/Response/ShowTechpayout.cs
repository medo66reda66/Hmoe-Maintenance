using Hmoe_Maintenance.Models;

namespace Hmoe_Maintenance.DTOs.Response
{
    public class ShowTechpayout
    {
        public int Id { get; set; }
        public int TechnicianProfileCopyId { get; set; }
        public decimal TechnicianProfileCopyRevenueShare { get; set; }
        public string? TechnicianProfileCopyname { get; set; }
        public string? TechnicianProfileCopyemail { get; set; }
        public string? TechnicianProfileCopyNationalId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PayoutDate { get; set; }
        public string? Notes { get; set; }
    }
}
