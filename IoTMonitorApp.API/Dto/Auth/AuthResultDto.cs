namespace IoTMonitorApp.API.Dto.Auth
{
    public class AuthResultDto
    {
        public string AccessToken { get; set; } = "";
        public string CsrfToken { get; set; } = "";
        public string RefreshToken { get; set; } = ""; // dùng nội bộ để set Cookie
    }
}
