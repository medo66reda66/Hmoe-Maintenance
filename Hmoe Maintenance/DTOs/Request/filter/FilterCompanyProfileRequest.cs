namespace Hmoe_Maintenance.DTOs.Request.filter
{
    public record FilterCompanyProfileRequest(
     string? Name = null,
     string? Governorate = null,
     string? City = null,
     bool? IsActive = null
    );
}
