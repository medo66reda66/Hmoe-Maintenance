using System.ComponentModel.DataAnnotations;

namespace Hmoe_Maintenance.DTOs.Response
{
    public class CompanyServiceResponse
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string ServiceCategoryName { get; set; } = string.Empty;
        public decimal InspectionPrice { get; set; }
        public decimal? StartingPrice { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
