namespace Hmoe_Maintenance.DTOs.Request.filter
{
    public record FilterTechPayoutRequest(
    string? Name,
    string? NationalId,
    string? Email,
    DateTime? FromDate,
    DateTime? ToDate
);
}
