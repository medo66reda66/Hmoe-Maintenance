namespace Hmoe_Maintenance.DTOs.Request.filter
{
    public record FilterTechnicianRequest(
      string? FullName = null,
      string? Email = null,
      string? NationalId = null,
      string? CompanyName = null,
      bool? IsAvailable = null,
      bool? IsActive = null,
      string? TechnicalService = null
  );
}
