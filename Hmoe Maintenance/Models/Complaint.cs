namespace Hmoe_Maintenance.Models
{
    public enum ComplaintStatus
    {
        Open = 1,
        UnderReview = 2,
        Resolved = 3,
        Rejected = 4,
        Closed = 5
    }
    public class Complaint
    {
        public int Id { get; set; }

        public int MaintenanceRequestId { get; set; }
        public MaintenanceRequest? MaintenanceRequest { get; set; } = default!;

        public string CreatedByUserId { get; set; } = default!;
        public ApplicationUser? CreatedByUser { get; set; } = default!;

        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;

        public ComplaintStatus Status { get; set; }

        public string? AdminResponse { get; set; }

        public string? HandledByAdminId { get; set; }
        public ApplicationUser? HandledByAdmin { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ResolvedAt { get; set; }
    }
}
