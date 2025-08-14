namespace IoTMonitorApp.API.Dto.Auth.Login
{
    public class LoginDto
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
