using Google.Apis.Auth;
using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.Dto.Auth.Login;
using IoTMonitorApp.API.Dto.Auth.Register;
using IoTMonitorApp.API.Dto.User;
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

        public async Task<(string token, UserDto user)> HandleGoogleLoginAsync(GoogleJsonWebSignature.Payload payload)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == payload.Email);

            if (user == null)
            {
                try
                {
                    user = new User
                    {
                        Id = Guid.NewGuid(),
                        Email = payload.Email,
                        FullName = payload.Name,
                        PhoneNumber = "027494234",
                        RoleId = 1, // Admin
                        AddressId = 1, // Default address
                        Role = "Admin",
                        AssignedDate = DateTime.UtcNow,
                        BirthOfDate = DateTime.Now,
                        PasswordHash = "" // user Google -> chưa có password
                    };

                    _db.Users.Add(user);
                    await _db.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }

            }

            var token = _jwtService.GenerateJwtToken(user);

            // map User -> UserDto
            var userDto = new UserDto
            {
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                PasswordHash = user.PasswordHash
            };

            return (token, userDto);
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
