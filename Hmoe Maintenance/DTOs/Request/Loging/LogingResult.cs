namespace Hmoe_Maintenance.DTOs.Request.Loging
{
    public enum LoginStatus
    {
        Success,
        UserNotFound,
        InvalidPassword,
        EmailNotConfirmed,
        LockedOut
    }
    public class LogingResult
    {
        public LoginStatus Status { get; set; }
        public string Token { get; set; }
        public string Validtoken { get; set; } = string.Empty;
        public string RefreshToken {  get; set; } = string.Empty;
        public string RefreshTokenTime { get; set; }
    }
}
