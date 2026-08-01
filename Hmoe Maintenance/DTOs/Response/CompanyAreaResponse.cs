using System.ComponentModel.DataAnnotations;

namespace Hmoe_Maintenance.DTOs.Response
{
    public class CompanyAreaResponse
    {
        public int Id { get; set; }
        public string Governorate { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? Area { get; set; } = null!;
        public bool IsActive { get; set; }
        public string companyName { get; set; } = null!;
        public string DiscriptionCompany { get; set; } =string.Empty;
        public string phoneNumberCompany { get; set; } = string.Empty;
        public string EmailCompany { get; set; } = string.Empty;
        public string? LicenseImageUrl { get; set; }=string.Empty;
        public string? CommercialRegistrationNumber { get; set; } = string.Empty;
        public string? CommercialRegistrationImageUrl { get; set; } = string.Empty;

    }
}
