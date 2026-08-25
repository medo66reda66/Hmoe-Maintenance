namespace Hmoe_Maintenance.Models
{
    public class Company
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser? applicationUser { get; set; } = null;

        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;

        public string? LogoUrl { get; set; }

        public string PhoneNumber { get; set; } = default!;
        public string? Email { get; set; }

        public bool IsApproved { get; set; }
        public bool IsActive { get; set; } = true;

        public decimal AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public string? RejectionReason { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string? LicenseImageUrl { get; set; }
        public string? CommercialRegistrationNumber { get; set; }
        public string? CommercialRegistrationImageUrl { get; set; }
        public int TechnicianCount { get; set; }
        public int? CompletedRequestsCount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<CompanyCoverageArea> CompanyCoverageAreas { get; set; }
    }
}
