namespace Hmoe_Maintenance.Models
{
    public class ServiceCategory
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        public string? IconUrl { get; set; }

        public bool IsActive { get; set; } = true;
    //    public ICollection<CompanyService>? CompanyServices { get; set; }
    //   = new List<CompanyService>();
    }

}

