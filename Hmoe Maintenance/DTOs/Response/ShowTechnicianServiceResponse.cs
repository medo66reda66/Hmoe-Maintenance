using Hmoe_Maintenance.Models;

namespace Hmoe_Maintenance.DTOs.Response
{
    public class ShowTechnicianServiceResponse
    {
        public int id { get; set; }
        public string Fullnametechnicia { get; set; }=string.Empty;
        public string NationalIdtec { get; set; } = default!;
        public string Emailtec { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? Profileurl { get; set; }
        public string? TechnicianDocumentUrl { get; set; }
        public string? FrontUrl { get; set; }
        public string? BackUrl { get; set; }
        public string? PhoneNumper { get; set; }
        public decimal? AverageRating { get; set; }
        public int? TotalCompletedJobs { get; set; }
        public string? ApprovedByUserId { get; set; }
        public decimal? RevenueShare { get; set; }
        public bool? IsAvailable { get; set; } = true;
        public bool? IsActive { get; set; } = true;

        public string CompanyName {  get; set; } = string.Empty;
        public string DescriptionCompany { get; set; } = default!;
        public string? EmailCompany { get; set; }
     
        public IEnumerable<Models.ServiceCategory>?  servicecategory { get; set; }
        public string servicecategoryname {  get; set; } = string.Empty;
        public string servicecategorydescription { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    }
}
