using Hmoe_Maintenance.Models;
using System.ComponentModel.DataAnnotations;

namespace Hmoe_Maintenance.DTOs.Request
{
    public class CreateadditionalcostRequest
    {
        [Required]
        public decimal LaborCost { get; set; }
        public decimal? PartsCost { get; set; } = 0;
        
        public string? Reason { get; set; } = default!;
        [Required]

        public List<IFormFile> ImageUrlS { get; set; } = default!;
    }
}
