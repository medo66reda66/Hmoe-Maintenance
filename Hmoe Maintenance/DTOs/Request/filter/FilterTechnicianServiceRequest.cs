namespace Hmoe_Maintenance.DTOs.Request.filter
{
    public record FilterTechnicianServiceRequest(
     string? FullName = null,
     string? Email = null,
     string? CompanyName = null,
     string? ServiceName = null,
     string? NationalId = null
 );
}
