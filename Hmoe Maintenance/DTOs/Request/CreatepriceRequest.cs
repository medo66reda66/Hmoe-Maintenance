namespace Hmoe_Maintenance.DTOs.Request
{
    public class CreatepriceRequest
    {
        public decimal InspectionPrice { get; set; }
        public decimal EstimatedPrice { get; set; }

        public string Notes { get; set; }
    }
}
