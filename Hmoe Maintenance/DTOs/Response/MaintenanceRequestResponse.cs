using Hmoe_Maintenance.Models;

namespace Hmoe_Maintenance.DTOs.Response
{
    public class MaintenanceRequestResponse
    {
        public int Id { get; set; }
        public string RequestNumber { get; set; } = string.Empty;

        // Customer
        public string CustomerId { get; set; } = string.Empty;

        // Company
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyEmail { get; set; } = string.Empty;

        // Service
        public int ServiceCategoryId { get; set; }
        public string ServiceName { get; set; } = string.Empty;

        // Location
        public string Governorate { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string BuildingNumber { get; set; } = string.Empty;
        public string? Floor { get; set; }
        public string Phone { get; set; } = string.Empty;

        // Request Details
        public string Description { get; set; } = string.Empty;

        // Appointment
        public DateTime PreferredDate { get; set; }
        public TimeSpan PreferredStartTime { get; set; }
        public TimeSpan PreferredEndTime { get; set; }

        // Technician
        public int? TechnicianId { get; set; }
        public string? TechnicianFullName { get; set; }
        public string? TechnicianEmail { get; set; }
        public string? TechnicianPhone { get; set; }

        // Prices
        public decimal InspectionPrice { get; set; }
        public decimal EstimatedPrice { get; set; }
        public decimal AdditionalCostsTotal { get; set; }
        public decimal FinalPrice { get; set; }

        // Status
        public MaintenanceRequestStatus Status { get; set; }

        // Payment
        public bool PaymentApproved { get; set; }
        public bool PaymentRejected { get; set; }

        // Technician Report
        public string? TechnicianReport { get; set; }

        // Dates
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
