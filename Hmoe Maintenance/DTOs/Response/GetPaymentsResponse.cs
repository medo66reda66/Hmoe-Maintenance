using Hmoe_Maintenance.DTOs.Response.filter;
using Hmoe_Maintenance.Models;

namespace Hmoe_Maintenance.DTOs.Response
{
    public class GetPaymentsResponse
    {
        public List<Payment> Payments { get; set; } = new();
        public FilterclientResponse Filter { get; set; } = new();
    }
}

