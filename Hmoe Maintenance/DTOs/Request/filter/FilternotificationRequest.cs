namespace Hmoe_Maintenance.DTOs.Request.filter
{
    public record FilternotificationRequest
    (
           string? RelatedEntityId,string? msg , bool? IsRead
    );
}
