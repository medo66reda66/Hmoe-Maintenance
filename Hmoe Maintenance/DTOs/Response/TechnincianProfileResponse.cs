using Hmoe_Maintenance.Models;

namespace Hmoe_Maintenance.DTOs.Response
{
    public class TechnincianProfileResponse
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public string NationalId { get; set; } = default!;
        public string ProfileImageUrl { get; set; } = default!;
        public string NationalIdFrontImageUrl { get; set; } = default!;
        public string NationalIdBackImageUrl { get; set; } = default!;

        public string? TechnicianDocumentUrl { get; set; } = default!;
        public IEnumerable<TechnicianService>? technicianServices { get; set; }
        public int YearsOfExperience { get; set; }
        public TechnicianStatus Status { get; set; }
        public string? ApprovedByUserId { get; set; }
        public decimal RevenueShare { get; set; }

        public string? Bio { get; set; }

        public decimal AverageRating { get; set; }
        public int TotalCompletedJobs { get; set; }

        public bool IsAvailable { get; set; } = true;
        public bool IsActive { get; set; } = true;


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
