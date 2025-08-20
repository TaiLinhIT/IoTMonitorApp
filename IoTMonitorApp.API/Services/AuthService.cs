using Google.Apis.Auth;
using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.Dto.Auth.Login;
using IoTMonitorApp.API.Dto.Auth.Register;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace IoTMonitorApp.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IJwtService _jwtService;

        public AuthService(AppDbContext db, IJwtService jwtService)
        {
            _db = db;
            _jwtService = jwtService;
        }

        // ✅ Đăng nhập thường (email/password)
        public async Task<string> LoginAsync(CreateLoginDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) throw new Exception("Người dùng không tồn tại");

            if (user.PasswordHash != HashPassword(dto.Password))
                throw new Exception("Sai mật khẩu");

            return _jwtService.GenerateJwtToken(user);
        }

        // ✅ Đăng ký
        public async Task RegisterAsync(RegisterDto dto)
        {
            var existing = await _db.Users.AnyAsync(u => u.Email == dto.Email);
            if (existing) throw new Exception("Email đã tồn tại");

            var user = new User
            {
                Email = dto.Email,
                FullName = dto.FullName,
                PasswordHash = HashPassword(dto.Password)
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        // ✅ Login Google
        public async Task<(string token, User user)> HandleGoogleLoginAsync(GoogleJsonWebSignature.Payload payload)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == payload.Email);

            if (user == null)
            {
                user = new User
                {
                    Email = payload.Email,
                    FullName = payload.Name,
                    PasswordHash = "" // user Google -> chưa có password
                };

                _db.Users.Add(user);
                await _db.SaveChangesAsync();
            }

            var token = _jwtService.GenerateJwtToken(user);
            return (token, user);
        }

        // ✅ Set password cho user Google sau này
        public async Task<bool> SetPasswordAsync(string email, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            user.PasswordHash = HashPassword(password);
            await _db.SaveChangesAsync();
            return true;
        }

        // 🔑 Helper: hash password
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
