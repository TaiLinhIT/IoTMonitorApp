using IoTMonitorApp.API.Dto;
using IoTMonitorApp.API.Models;
using System.Security.Claims;

namespace IoTMonitorApp.API.IServices
{
    public interface IAuthService
    {
        Task<(string token, User user)> HandleGoogleLoginAsync(ClaimsPrincipal principal);//Đăng nhập bằng tài khoản Google (OAuth2)
        string GenerateJwtToken(string email);//Tạo JSON Web Token (JWT)
        Task RegisterAsync(RegisterDto dto);//Đăng ký người dùng thủ công (không qua Google)
        Task<string> LoginAsync(LoginDto dto);//Đăng nhập người dùng thủ công (không qua Google)
        Task<bool> SetPasswordAsync(string email, string password);
    }
}
