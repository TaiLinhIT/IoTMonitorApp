using Google.Apis.Auth;
using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.Dto.Auth;
using IoTMonitorApp.API.Dto.Auth.Login;
using IoTMonitorApp.API.Dto.Auth.Register;
using IoTMonitorApp.API.Dto.User;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text.Encodings.Web;

namespace IoTMonitorApp.API.Services
{
    /// <summary>
    /// Xử lý các nghiệp vụ xác thực và ủy quyền người dùng.
    /// Bao gồm: login, register, Google login, refresh token, revoke, set password.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IJwtService _jwtService;

        public AuthService(AppDbContext db, IJwtService jwtService)
        {
            _db = db;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Đăng nhập với email & password.
        /// Trả về Access Token + Refresh Token.
        /// </summary>
        public async Task<AuthResultDto> LoginAsync(CreateLoginDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) throw new Exception("Người dùng không tồn tại");

            if (!VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Sai mật khẩu");

            return await GenerateTokensAsync(user);
        }

        /// <summary>
        /// Đăng ký tài khoản mới.
        /// Hash mật khẩu bằng PBKDF2.
        /// </summary>
        public async Task RegisterAsync(RegisterDto dto)
        {
            var existing = await _db.Users.AnyAsync(u => u.Email == dto.Email);
            if (existing) throw new Exception("Email đã tồn tại");

            CreatePasswordHash(dto.Password, out string hash, out string salt);

            var user = new User
            {
                Email = Sanitize(dto.Email),
                FullName = Sanitize(dto.FullName),
                PasswordHash = hash,
                PasswordSalt = salt,
                RoleId = 1
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Google Login - nếu chưa có user thì tạo mới.
        /// Trả về token + user info.
        /// </summary>
        public async Task<(string token, UserDto user)> HandleGoogleLoginAsync(GoogleJsonWebSignature.Payload payload)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == payload.Email);

            if (user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = payload.Email,
                    FullName = payload.Name,
                    PhoneNumber = "",
                    RoleId = 2,
                    AssignedDate = DateTime.UtcNow,
                    PasswordHash = "",
                    PasswordSalt = ""
                };

                _db.Users.Add(user);
                await _db.SaveChangesAsync();
            }

            var token = _jwtService.GenerateJwtToken(user);

            var userDto = new UserDto
            {
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber
            };

            return (token, userDto);
        }

        /// <summary>
        /// Đặt mật khẩu cho user Google.
        /// </summary>
        public async Task<bool> SetPasswordAsync(string email, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            CreatePasswordHash(password, out string hash, out string salt);
            user.PasswordHash = hash;
            user.PasswordSalt = salt;

            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Sinh access + refresh token và lưu refresh token vào DB.
        /// </summary>
        private async Task<AuthResultDto> GenerateTokensAsync(User user)
        {
            // 1. Sinh access token
            var accessToken = _jwtService.GenerateJwtToken(user);

            // 2. Sinh refresh token
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            // 3. Sinh CSRF token
            var csrfToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

            // 4. Lưu refresh token vào DB
            var refresh = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };
            _db.RefreshTokens.Add(refresh);
            await _db.SaveChangesAsync();

            // 5. Trả về AuthResultDto
            return new AuthResultDto
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                Expiry = DateTime.UtcNow.AddMinutes(15),
                CsrfToken = csrfToken
                //,
                //UserRole = _db.Users.Join(_db.Roles, u => u.RoleId, r => r.Id, (u, r) => new { u, r })
                //                .Where(ur => ur.u.Id == user.Id)
                //                .Select(ur => ur.r.Name)
                //                .FirstOrDefault() ?? ""
            };
        }


        /// <summary>
        /// Làm mới Access Token bằng Refresh Token.
        /// </summary>
        public async Task<AuthResultDto?> RefreshTokenAsync(string refreshToken, string csrfToken)
        {
            var stored = await _db.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == refreshToken && !r.IsRevoked);

            if (stored == null || stored.ExpiryDate < DateTime.UtcNow)
                return null;

            return await GenerateTokensAsync(stored.User);
        }

        /// <summary>
        /// Revoke refresh token (logout).
        /// </summary>
        public async Task<bool> RevokeTokenAsync(string refreshToken)
        {
            var stored = await _db.RefreshTokens.FirstOrDefaultAsync(r => r.Token == refreshToken);
            if (stored == null) return false;

            stored.IsRevoked = true;
            await _db.SaveChangesAsync();
            return true;
        }

        #region Password Hashing (PBKDF2)
        private void CreatePasswordHash(string password, out string hash, out string salt)
        {
            using var hmac = new Rfc2898DeriveBytes(password, 16, 100000, HashAlgorithmName.SHA512);
            salt = Convert.ToBase64String(hmac.Salt);
            hash = Convert.ToBase64String(hmac.GetBytes(64));
        }

        private bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            using var hmac = new Rfc2898DeriveBytes(password, saltBytes, 100000, HashAlgorithmName.SHA512);
            var computedHash = Convert.ToBase64String(hmac.GetBytes(64));
            return computedHash == storedHash;
        }
        #endregion

        #region Security Helpers
        private string Sanitize(string input)
        {
            return string.IsNullOrEmpty(input) ? input : HtmlEncoder.Default.Encode(input);

        }


        #endregion
    }
}
