using System.ComponentModel.DataAnnotations;

namespace Hmoe_Maintenance.DTOs.Response
{
    public class ProfileUserResponse
    {
        public string Id { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty; // Home, Work

        public string Governorate { get; set; } = string.Empty; // Cairo
        public string City { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;

        public string BuildingNumber { get; set; } = string.Empty;
        public string? Floor { get; set; }
        public string? ApartmentNumber { get; set; }
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

    }
}
