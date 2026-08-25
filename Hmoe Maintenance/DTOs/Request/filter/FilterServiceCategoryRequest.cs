namespace Hmoe_Maintenance.DTOs.Request.filter
{
    public record FilterServiceCategoryRequest(
     string? Name = null,
     bool? IsActive = null
 );
}
