using Hmoe_Maintenance.Models;

namespace Hmoe_Maintenance.DTOs.Request
{
    public class UpdateMaintenanceRequest
    {
        // العميل
        public string CustomerId { get; set; } = default!;

        // الشركة والخدمة
        public int CompanyId { get; set; }

        public int ServiceCategoryId { get; set; }

        // مكان الصيانة
        public int AddressId { get; set; }
        public Address Address { get; set; } = default!;
        public string Governorate { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string BuildingNumber { get; set; } = default!;
        public string? Floor { get; set; }
        public string Phone { get; set; }

        // وصف المشكلة
        public string Description { get; set; } = default!;

        // الموعد
        public DateTime PreferredDate { get; set; }
        public TimeSpan PreferredStartTime { get; set; }

        // الفني المعيّن
        public int? AssignedTechnicianId { get; set; }

        // تقرير الفني في النهاية
        public string? TechnicianReport { get; set; }
        public string ImageUrl { get; set; } = default!;
        public bool IsBeforeWork { get; set; }
    }
}
