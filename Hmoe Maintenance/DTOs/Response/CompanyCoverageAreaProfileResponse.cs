namespace Hmoe_Maintenance.DTOs.Response
{
    public class CompanyCoverageAreaProfileResponse
    {
        public int Id { get; set; }

        public string Governorate { get; set; } = default!;

        public string City { get; set; } = default!;

        public string? Area { get; set; }

        public bool IsActive { get; set; }
    }
}

