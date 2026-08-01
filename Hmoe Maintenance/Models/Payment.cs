using Stripe;

namespace Hmoe_Maintenance.Models
{
    public enum PaymentStatus
    {
        Pending = 1,
        Paid = 2,
        Failed = 3,
        Refunded = 4,
        Cancelled = 5
    }

    public enum PaymentMethod
    {
        Cash = 1,
        Card = 2,
        Wallet = 3
    }
    public class Payment
    {
        public int Id { get; set; }

        // طلب الصيانة الذي تم الدفع له
        public int MaintenanceRequestId { get; set; }
        public MaintenanceRequest? MaintenanceRequest { get; set; } = default!;

        // المبلغ المدفوع
        public decimal Amount { get; set; }

        // كاش أم كارت؟
        public PaymentMethod PaymentMethod { get; set; }

        // حالة الدفع
        public PaymentStatus Status { get; set; }

        // رقم العملية من بوابة الدفع، مهم في الدفع بالكارت
        public string? TransactionId { get; set; }

        // اسم بوابة الدفع، مثل Stripe أو Paymob
        public string? GatewayName { get; set; }

        // وقت إنشاء عملية الدفع
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // وقت نجاح الدفع
        public DateTime? PaidAt { get; set; }
    
}
}
