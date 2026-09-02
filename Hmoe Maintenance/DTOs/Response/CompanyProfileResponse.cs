using Hmoe_Maintenance.Models;

namespace Hmoe_Maintenance.DTOs.Response
{
    public class CompanyProfileResponse
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? LogoUrl { get; set; }

        public string PhoneNumber { get; set; } = default!;
        public string? Email { get; set; }

        public decimal AverageRating { get; set; }
        public int TotalReviews { get; set; }

        public int TechnicianCount { get; set; }
        public int? CompletedRequestsCount { get; set; }

        public bool IsActive { get; set; }

        public List<TechnicianincompanyProfileResponse>? Technicians { get; set; } = new List<TechnicianincompanyProfileResponse>();
        public List<CompanyCoverageAreaProfileResponse>? CoverageAreas { get; set; } = new List<CompanyCoverageAreaProfileResponse>();

        public List<CompanyServiceResponse>? companyServices { get; set; }
    }
}
