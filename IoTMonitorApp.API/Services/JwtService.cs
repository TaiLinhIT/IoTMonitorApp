using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IoTMonitorApp.API.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _db;

        public JwtService(IConfiguration configuration, AppDbContext appDbContext)
        {
            _configuration = configuration;
            _db = appDbContext;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // ⏰ access token sống 1h
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return principal;
            }
            catch
            {
                return null;
            }
        }
        public string GenerateJwtToken(User user)
        {
            //Lấy vai trò của user từ CSDL
            var role = _db.Roles.Where(x => x.Id == user.RoleId).Select(x => x.Name).FirstOrDefault();
            //Claims là những thông tin (key-value) mà ta muốn lưu trong token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),//Email của user
                new Claim(ClaimTypes.Name, user.FullName ?? ""),//Full name của user
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), //Id của User
                new Claim(ClaimTypes.Role, role) //Vai trò của user
            };
            //Tạo chữ ký cho token
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Authentication:Jwt:Key"]) //secret key từ appsettings.json
            );
            // Tạo chữ ký số
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //Sử dụng thuật toán HmacSha256
            // Tạo token


            var token = new JwtSecurityToken(
                issuer: _configuration["Authentication:Jwt:Issuer"],// Ai là người phát hành token
                audience: _configuration["Authentication:Jwt:Audience"],//Ai là người được phép sử dụng token
                claims: claims,//Những thông tin muốn lưu trong token
                expires: DateTime.UtcNow.AddHours(2),//Thời gian hết hạn của token
                signingCredentials: creds//chữ ký số
            );

            // Trả về token dưới dạng chuỗi
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
