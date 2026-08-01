namespace Hmoe_Maintenance.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int MaintenanceRequestId { get; set; }
        public MaintenanceRequest? MaintenanceRequest { get; set; } = default!;

        public string CustomerId { get; set; } = default!;
        public ApplicationUser? Customer { get; set; } = default!;

        public int CompanyId { get; set; }
        public Company? Company { get; set; } = default!;

        public int? TechnicianProfileId { get; set; }
        public TechnicianProfile? TechnicianProfile { get; set; }

        public int Rating { get; set; } // من 1 إلى 5

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
