namespace Hmoe_Maintenance.Models
{
    public class Address
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser? applicationUser { get; set; }=null;

        public string Title { get; set; } = string.Empty; // Home, Work

        public string Governorate { get; set; } = string.Empty; // Cairo
        public string City { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;

        public string BuildingNumber { get; set; } = string.Empty;
        public string? Floor { get; set; }
        public string? ApartmentNumber { get; set; }

        public string? Landmark { get; set; }

        public decimal? Latitude { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
