namespace Hmoe_Maintenance.Models
{
    public class CompanyCoverageArea
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; } = default!;

        public string Governorate { get; set; } = default!;
        public string City { get; set; } = default!;
        public string? Area { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
