using Google.Apis.Auth;
using IoTMonitorApp.API.Dto.Auth.Login;
using IoTMonitorApp.API.Dto.Auth.Register;
using IoTMonitorApp.API.Models;

public interface IAuthService
{
    Task<string> LoginAsync(CreateLoginDto dto);
    Task RegisterAsync(RegisterDto dto);
    Task<(string token, User user)> HandleGoogleLoginAsync(GoogleJsonWebSignature.Payload payload);
    Task<bool> SetPasswordAsync(string email, string password);
}
