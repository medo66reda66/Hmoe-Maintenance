using Hmoe_Maintenance.Models;

namespace Hmoe_Maintenance.DTOs.Response
{
    using System.ComponentModel.DataAnnotations;

    public class ShowReviewsResponse
    {
        public int MaintenanceRequestId { get; set; }
        public string? Requestnum { get; set; }
        public string? MaintenanceRequestCity { get; set; }
        public string? MaintenanceRequestGovernorate { get; set; }

        public string CustomerId { get; set; } = default!;
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
       

        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }

        public int? TechnicianProfileId { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
