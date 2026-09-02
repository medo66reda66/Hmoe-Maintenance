namespace Hmoe_Maintenance.Models
{
    public enum TechnicianStatusCopy
    {
        Pending,
        Approved,
        Rejected,
        Suspended
    }
    public class TechnicianProfileCopy
    {
        public int Id { get; set; }

        public string UserId { get; set; } = default!;
        public ApplicationUser? User { get; set; } = default!;

        public int? CompanyCopyId { get; set; }
        public CompanyCopy? CompanyCopy { get; set; }

        public string Fullname { get; set; }
        public string NationalId { get; set; } = default!;
        public string ProfileImageUrl { get; set; } = default!;
        public string NationalIdFrontImageUrl { get; set; } = default!;
        public string NationalIdBackImageUrl { get; set; } = default!;

        public string? TechnicianDocumentUrl { get; set; }= default!;
        public int YearsOfExperience { get; set; }
        public TechnicianStatusCopy Status { get; set; }
        public string? ApprovedByUserId { get; set; }
        public decimal RevenueShare { get; set; }

        public string? Bio { get; set; }
        public string PhoneNumper { get; set; }
        public string Email { get; set; } = string.Empty;


        public decimal AverageRating { get; set; }
        public int TotalCompletedJobs { get; set; }

        public bool IsAvailable { get; set; } = true;
        public bool IsActive { get; set; } = true;


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }

    }
}

