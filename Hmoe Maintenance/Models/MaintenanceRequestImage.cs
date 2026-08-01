namespace Hmoe_Maintenance.Models
{
    public class MaintenanceRequestImage
    {
        public int Id { get; set; }

        public int MaintenanceRequestId { get; set; }
        public MaintenanceRequest? MaintenanceRequest { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;

        public string UploadedByUserId { get; set; } = default!;
        public ApplicationUser? UploadedByUser { get; set; } = default!;

        public bool IsBeforeWork { get; set; }
        public bool IsAfterWork { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
