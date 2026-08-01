namespace Hmoe_Maintenance.DTOs.Response
{
    public class ShowTechnicianServiceResponse
    {
        public int id { get; set; }
        public string Fullnametechnicia { get; set; }=string.Empty;
        public string NationalIdtec { get; set; } = default!;
        public string Emailtec { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string CompanyName {  get; set; } = string.Empty;
        public string DescriptionCompany { get; set; } = default!;
        public string? EmailCompany { get; set; }
        public string Nameservicecategory { get; set; } = default!;
        public string? Descriptionservicecategory { get; set; }

    }
}
