using System.ComponentModel.DataAnnotations;

namespace Hmoe_Maintenance.DTOs.Request
{
    public class UpdateadditionalcostRequest
    {
        public decimal? LaborCost { get; set; }
        public decimal? PartsCost { get; set; } = 0;

        public string? Reason { get; set; } = default!;

        public List<IFormFile>? ImageUrlS { get; set; } = default!;
    }
}
