namespace Hmoe_Maintenance.DTOs.Request.filter
{
    public record FilterReviewRequest
    (
        int? MinRating,
        int? MaxRating,
        string? CustomerId,
        string? requestnum
    );
}
