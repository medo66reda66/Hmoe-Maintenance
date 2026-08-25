using Hmoe_Maintenance.Models;

namespace Hmoe_Maintenance.DTOs.Response
{
    public class PaymentResponse
    {
        // Payment
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; }
        public string? TransactionId { get; set; }
        public string? SessionId { get; set; }
        public string? GatewayName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string? StripePaymentIntentId { get; set; }
        public string? StripeSessionId { get; set; }

        // Maintenance Request
        public int MaintenanceRequestId { get; set; }
        public string RequestNumber { get; set; } = string.Empty;
        public string MaintenanceDescription { get; set; } = string.Empty;
        public MaintenanceRequestStatus MaintenanceStatus { get; set; }

        public string ServiceName { get; set; } = string.Empty;

        public string Governorate { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string BuildingNumber { get; set; } = string.Empty;
        public string? Floor { get; set; }

        public DateTime PreferredDate { get; set; }
        public TimeSpan PreferredStartTime { get; set; }
        public TimeSpan PreferredEndTime { get; set; }

        public decimal InspectionPrice { get; set; }
        public decimal EstimatedPrice { get; set; }
        public decimal AdditionalCostsTotal { get; set; }
        public decimal FinalPrice { get; set; }

        // Client / Customer
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerFullName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
    }
}
