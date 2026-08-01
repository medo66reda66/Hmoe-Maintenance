namespace Hmoe_Maintenance.Models
{
    public class ApplicationuserOtp
    {
        public string id { get; set; }
        public string Otp { get; set; }
        public DateTime Validto { get; set; }
        public DateTime CreateAt { get; set; }
        public bool Isvalid { get; set; }
        public string Applicationuserid { get; set; }
        public ApplicationUser Applicationuser { get; set; }
    }
}
