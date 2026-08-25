namespace Hmoe_Maintenance.DTOs.Response
{
    public class PaginationResponse<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<T> Data { get; set; } = new();
    }
}

