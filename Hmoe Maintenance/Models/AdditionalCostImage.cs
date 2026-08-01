namespace Hmoe_Maintenance.Models
{
    public class AdditionalCostImage
    {
        public int Id { get; set; }

        public int AdditionalCostRequestId { get; set; }
        public AdditionalCostRequest? AdditionalCostRequest { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
