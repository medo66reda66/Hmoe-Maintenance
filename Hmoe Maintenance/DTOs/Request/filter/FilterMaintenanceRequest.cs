namespace Hmoe_Maintenance.DTOs.Request.filter
{
    public record FilterMaintenanceRequest(
        string? RequestNumber = null,
        string? CompanyName = null,
        string? Governorate = null,
        string? City = null
     );
}
