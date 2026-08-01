namespace Hmoe_Maintenance.Models
{
    public enum AdditionalCostStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3,
        Cancelled = 4
    }
    public class AdditionalCostRequest
    {
        public int Id { get; set; }

        public int MaintenanceRequestId { get; set; }
        public MaintenanceRequest? MaintenanceRequest { get; set; } = default!;

        public int TechnicianProfileId { get; set; }
        public TechnicianProfile? TechnicianProfile { get; set; } = default!;

        public decimal LaborCost { get; set; }
        public decimal PartsCost { get; set; }

        public decimal TotalAmount { get; set; }

        public string Reason { get; set; } = default!;

        public AdditionalCostStatus Status { get; set; }

        public string? CustomerResponseNote { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RespondedAt { get; set; }

        public ICollection<AdditionalCostImage>? Images { get; set; }
            = new List<AdditionalCostImage>();
    }
}
