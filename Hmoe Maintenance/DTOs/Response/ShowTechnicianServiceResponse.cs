namespace Hmoe_Maintenance.DTOs.Response
{
    public class ShowTechnicianServiceResponse
    {
        public int id { get; set; }
        public string Fullnametechnicia { get; set; }=string.Empty;
        public string NationalIdtec { get; set; } = default!;
        public string Emailtec { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? PhoneNumper { get; set; }
        public decimal? AverageRating { get; set; }
        public int? TotalCompletedJobs { get; set; }
        public string? ApprovedByUserId { get; set; }
        public decimal? RevenueShare { get; set; }
        public bool? IsAvailable { get; set; } = true;
        public bool? IsActive { get; set; } = true;

        public string tecnicalservice { get; set; } = default!;

        public string CompanyName {  get; set; } = string.Empty;
        public string DescriptionCompany { get; set; } = default!;
        public string? EmailCompany { get; set; }
     
        public string? Descriptionservicecategory { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    }
}
