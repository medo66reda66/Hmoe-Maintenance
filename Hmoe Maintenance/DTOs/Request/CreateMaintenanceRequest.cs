using Hmoe_Maintenance.Models;
using System.ComponentModel.DataAnnotations;

namespace Hmoe_Maintenance.DTOs.Request
{
    public class CreateMaintenanceRequest
    {

        // الشركة والخدمة
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public int CompanyCopyId { get; set; }
        [Required]
        public int ServiceCategoryId { get; set; }
        [Required]

        // مكان الصيانة
        public int AddressId { get; set; }
        [Required]
        public string Governorate { get; set; } = default!;
        [Required]
        public string City { get; set; } = default!;
        [Required]
        public string Street { get; set; } = default!;
        [Required]
        public string BuildingNumber { get; set; } = default!;
        public string? Floor { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]

        // وصف المشكلة
        public string Description { get; set; } = default!;

        // الموعد
        [Required]
        public DateTime PreferredDate { get; set; }
        // تقرير الفني في النهاية
        [Required]
        public List<IFormFile> ImageUrlS { get; set; } = default!;
        // تقرير الفني في النهاية

    }
}
