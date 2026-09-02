namespace Hmoe_Maintenance.DTOs.Response
{
    public class ShowCompanyServiceResponse
    {
        public int? Id { get; set; }
        public string ServiceName { get; set; } = default!;
        public int? ServiceId { get; set; }
        public string? ServiceDescription { get; set; }

        public string? IconUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public decimal InspectionPrice { get; set; }
        public string? companyName { get; set; }
        public string? companyDescription { get; set; }
        public decimal? StartingPrice { get; set; }


    }
}
