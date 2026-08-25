using Hmoe_Maintenance.Models;

namespace Hmoe_Maintenance.DTOs.Request.filter
{
    public record FilterclientRequest
    (
        string? ClientName = null,
        string? RequestNumber = null,
        PaymentStatus? Status = null,
        PaymentMethod? PaymentMethod = null
    );
    
}
