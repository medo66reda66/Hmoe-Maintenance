namespace Hmoe_Maintenance.Models
{
    public class TechnicianService
    {
       
            public int Id { get; set; }

            public int TechnicianProfileId { get; set; }
            public TechnicianProfile? TechnicianProfile { get; set; } = default!;

            public int ServiceCategoryId { get; set; }
            public ServiceCategory? ServiceCategory { get; set; } = default!;
        
    }
}
