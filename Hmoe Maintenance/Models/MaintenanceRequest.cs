namespace Hmoe_Maintenance.Models
{
    public enum MaintenanceRequestStatus
    {
        PendingCompanyApproval = 1,
        CompanyAccepted = 2,
        TechnicianAssigned = 3,
        TechnicianOnTheWay = 4,
        TechnicianArrived = 5,
        WorkInProgress = 6,
        WaitingForCustomerApprovaladditioncost = 7,
        PaymentPending = 8,
        Completed = 9,
        Cancelled = 10,
        Disputed = 11,
       Companyrejectedrequest = 12,
        WorkCompleted=13,
        WaitingCustomerOfferResponseprice=14,
        clientApproveprice=15,
        clientrejectedprice=16,

    }
    public class MaintenanceRequest
    {
        public int Id { get; set; }
        public string RequestNumber { get; set; } = default!;
        // العميل
        public string CustomerId { get; set; } = default!;
        public ApplicationUser Customer { get; set; } = default!;
        // الشركة والخدمة
        public int CompanycopyId { get; set; }
        public CompanyCopy? CompanyCopy { get; set; }= default!;
        public int ServiceCategoryId { get; set; }
        public ServiceCategory ServiceCategory { get; set; } = default!;
        // مكان الصيانة
        public int AddressId { get; set; }
        public Address Address { get; set; } = default!;
        //location
        public string Governorate { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string BuildingNumber { get; set; } = default!;
        public string? Floor { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; } = default!;
        // الموعد
        public DateTime PreferredDate { get; set; }
        public TimeSpan PreferredStartTime { get; set; }
        public TimeSpan PreferredEndTime { get; set; }
        // الفني المعيّن
        public int? AssignedTechnicianId { get; set; }
        public TechnicianProfile? AssignedTechnician { get; set; }
        public int technicianProfileCopyId { get; set; }
        public TechnicianProfileCopy? technicianProfileCopy { get; set; }

        // الأسعار
        public decimal InspectionPrice { get; set; }
        public decimal EstimatedPrice { get; set; }
        public decimal AdditionalCostsTotal { get; set; }
        public decimal FinalPrice { get; set; }
        // الحالة
        public MaintenanceRequestStatus Status { get; set; }
        // تقرير الفني في النهاية
        public string? TechnicianReport { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }

        public bool PaymentApproved { get; set; }
        public bool PaymentRejected { get; set; }

        public ICollection<MaintenanceRequestImage>? Images { get; set; }
            = new List<MaintenanceRequestImage>();

        public ICollection<MaintenanceRequestStatusHistory>? StatusHistory { get; set; }
            = new List<MaintenanceRequestStatusHistory>();

        public ICollection<AdditionalCostRequest>? AdditionalCostRequests { get; set; }
            = new List<AdditionalCostRequest>();

        public ICollection<Payment>? Payments { get; set; }
            = new List<Payment>();

        public ICollection<Review>? Reviews { get; set; }
            = new List<Review>();

        public ICollection<Complaint>? Complaints { get; set; }
            = new List<Complaint>();
    }
}
