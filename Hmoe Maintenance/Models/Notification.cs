namespace Hmoe_Maintenance.Models
{
    public enum NotificationType
    {
        General = 1,

        // Company
        CompanyPendingApproval = 2,
        CompanyApproved = 3,
        CompanyRejected = 4,

        // Technician Application
        TechnicianApplicationReceived = 5,   // الشركة استلمت طلب تقديم من فني
        TechnicianApplicationupdate = 6,   // الشركة استلمت طلب update من فني
        TechnicianApplicationApproved = 7,   // تم قبول الفني
        TechnicianApplicationRejected = 8,// تم رفض الفني

        // Maintenance Requests
        PendingCompanyApproval=9,
        CompanyAccepted=10,
        TechnicianAssigned=11,
        TechnicianOnTheWay=12,
  
        Completed =13,
        AdditionalCostRequested = 14,

        // Payment
        PaymentSuccess = 15,

        // Complaint
        ComplaintUpdated = 16
            , ComplaintApproved = 17,
        clientApproved = 18,
        clientRejected = 19,
        AdditionalCostRejected=20,
        AdditionalCostApproved=21,
        TechnicianArrive =22,
        WorkCompleted=23,
        WaitingCustomerOfferResponseprice=24,
        PriceOfferAccepted=25,
        PriceOfferRejected=26,
        WorkInProgress=27,
        WorkCancelled=28,

        UpdateAdditionalCostRequested=29,
    }

    public class Notification
    {
        public int Id { get; set; }

        public string UserId { get; set; } = default!;
        public ApplicationUser? User { get; set; } = default!;

        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;

        public NotificationType Type { get; set; }

        public bool IsRead { get; set; }

        public string? RelatedEntityId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
