namespace Hmoe_Maintenance.DTOs.Request.filter
{
    public record FilterUsersRequest
    (
        string? id , string? Fullname , string? Email ,bool? loukout 
        );
}
