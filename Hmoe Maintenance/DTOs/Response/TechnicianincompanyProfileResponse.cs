using Hmoe_Maintenance.Models;

namespace Hmoe_Maintenance.DTOs.Response
{
    public class TechnicianincompanyProfileResponse
    {
        public int Id { get; set; }

        public string Fullname { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;

        public int YearsOfExperience { get; set; }

        public string? Bio { get; set; }

        public decimal AverageRating { get; set; }

        public int TotalCompletedJobs { get; set; }
        public decimal? revenueShare { get; set; }
        public bool IsActive { get; set; }

        public bool IsAvailable { get; set; }

        List<ServiceCategory> TechnicianServices { get; set; } = new List<ServiceCategory>();

    }
}
