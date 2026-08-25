using Hmoe_Maintenance.Models;

namespace Hmoe_Maintenance.DTOs.Response.filter
{
    public class FilterclientResponse
    {

        public string? ClientName { get; set; }
        public string? RequestNumber { get; set; }
        public PaymentStatus? Status { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }

    }
}

