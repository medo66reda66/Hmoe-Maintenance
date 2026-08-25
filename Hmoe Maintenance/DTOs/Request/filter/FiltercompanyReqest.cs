namespace Hmoe_Maintenance.DTOs.Request.filter
{
    public record FiltercompanyReqest
    (
        string? userOwnername, string? name, string? email,bool? IsApprove , bool? isactive ,
        string? Governorate,string? City ,bool? IsActiveArea
    );
}
