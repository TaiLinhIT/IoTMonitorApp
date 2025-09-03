namespace IoTMonitorApp.API.Dto.Auth
{
    public class AuthResultDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiry { get; set; }  // ✅ Thêm property này
        //public string CsrfToken { get; set; } = "";
    }

}
