using System.ComponentModel.DataAnnotations;

namespace Hmoe_Maintenance.DTOs.Request
{
    public class AddressRequest
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Governorate is required.")]
        public string Governorate { get; set; } = string.Empty;
        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; } = string.Empty;
        [Required(ErrorMessage = "Area is required.")]
        public string Area { get; set; } = string.Empty;
        [Required(ErrorMessage = "Street is required.")]
        public string Street { get; set; } = string.Empty;
        [Required(ErrorMessage = "Building number is required.")]
        public string BuildingNumber { get; set; } = string.Empty;
        public string? Floor { get; set; }
        public string? ApartmentNumber { get; set; }
        public string? Landmark { get; set; }
        public decimal? Latitude { get; set; }
    }
}
