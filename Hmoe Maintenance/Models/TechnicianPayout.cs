namespace Hmoe_Maintenance.Models
{
    public class TechnicianPayout
    {
        public int Id { get; set; }
        public int TechnicianProfileCopyId { get; set; }
        public TechnicianProfileCopy? TechnicianProfileCopy { get; set; }
        public decimal Amount { get; set; }
        public DateTime PayoutDate { get; set; }
        public string? Notes { get; set; }
    }
}
