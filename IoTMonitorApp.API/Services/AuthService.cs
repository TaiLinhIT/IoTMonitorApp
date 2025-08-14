using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.Dto.Auth.Login;
using IoTMonitorApp.API.Dto.Auth.Register;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IoTMonitorApp.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _config;
        private readonly PasswordHasher<User> _passwordHasher;
        public AuthService(AppDbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;
            _passwordHasher = new PasswordHasher<User>();
        }
        public async Task RegisterAsync(RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Email và password là bắt buộc.");

            if (_dbContext.Users.Any(u => u.Email == dto.Email))
                throw new Exception("Email đã được sử dụng.");

            var user = new User
            {
                Email = dto.Email,
                FullName = dto.FullName,
                Role = "User",
                CreatedDate = DateTime.UtcNow,
                ImageUrl = null,
                GoogleId = null


            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
            _dbContext.Users.Add(user);
            try
            {
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine("DbUpdateException: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("InnerException: " + ex.InnerException.Message);
                }

                // Có thể log hoặc throw tiếp tùy yêu cầu
                throw;
            }
        }

        public async Task<LoginDto> LoginAsync(CreateLoginDto dto)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.Email == dto.Email);
            var role = user?.Role;
            if (user == null)
                throw new Exception("Email không tồn tại.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result != PasswordVerificationResult.Success)
                throw new Exception("Mật khẩu không đúng.");

            var token = GenerateJwtToken(dto.Email);
            return new LoginDto
            {
                Token = token,
                Email = user.Email,
                Role = role
            };
        }
        public async Task<(string token, User user)> HandleGoogleLoginAsync(ClaimsPrincipal principal)
        {
            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = principal.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email)) throw new Exception("Email không tồn tại trong tài khoản Google.");

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    FullName = name,
                    CreatedDate = DateTime.UtcNow
                };

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
            }

            var token = GenerateJwtToken(email);
            return (token, user);
        }

        public string GenerateJwtToken(string email)
        {
            var jwtSettings = _config.GetSection("Authentication:Jwt");
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> SetPasswordAsync(string email, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
