using System.ComponentModel.DataAnnotations.Schema;

namespace Hmoe_Maintenance.Models
{
    
    public class CompanyService
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }
        public Company? Company { get; set; } = default!;

        public int ServiceCategoryId { get; set; }
        public ServiceCategory? ServiceCategory { get; set; } = default!;

        public decimal InspectionPrice { get; set; }

        public decimal? StartingPrice { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
