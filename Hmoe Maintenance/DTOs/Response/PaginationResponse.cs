namespace Hmoe_Maintenance.DTOs.Response
{
    public class PaginationResponse<Trequest, Tresponse>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<Trequest> Datarequest { get; set; } = new();
        public Tresponse? Dataresponse { get; set; } = default;
    }
}

