using IoTMonitorApp.API.Dto.Auth.Register;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static IoTMonitorApp.API.Services.AuthService;

namespace IoTMonitorApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly CsrfService _csrfService;

        public AuthController(IConfiguration config, IAuthService authService, IJwtService jwtService, CsrfService csrfService)
        {
            _config = config;
            _authService = authService;
            _jwtService = jwtService;
            _csrfService = csrfService;
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                return Unauthorized(new { message = "Missing refresh token" });

            var csrfHeader = Request.Headers["X-CSRF-Token"].FirstOrDefault();
            if (csrfHeader == null)
                return Unauthorized(new { message = "Missing CSRF token" });

            var result = await _authService.RefreshTokenAsync(refreshToken, csrfHeader);
            if (result == null)
                return Unauthorized(new { message = "Invalid token" });

            return Ok(new
            {
                accessToken = result.AccessToken,
                csrfToken = result.CsrfToken
            });
        }


        // ---------------- Email + Password ----------------
        [HttpPost("login")]
        public IActionResult Login([FromBody] Dto.Auth.LoginRequest request)
        {
            // 🔹 Thực tế bạn sẽ check username/password tại đây
            if (request.Username == "admin" && request.Password == "123456")
            {
                // Phát CSRF token
                var csrfToken = _csrfService.GenerateToken();

                // Set cookie HttpOnly
                Response.Cookies.Append("X-CSRF-TOKEN", csrfToken, new CookieOptions
                {
                    HttpOnly = false, // ❌ cần false để JS đọc gửi lại header
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });

                return Ok(new { message = "Login success" });
            }

            return Unauthorized();
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                await _authService.RegisterAsync(dto);
                return Ok(new { message = "Đăng ký thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ---------------- Google Login (SPA flow) ----------------
        [HttpPost("login-google")]
        public async Task<IActionResult> LoginGoogle([FromBody] GoogleLoginDto dto)
        {
            try
            {
                var payload = await Google.Apis.Auth.GoogleJsonWebSignature.ValidateAsync(dto.IdToken);

                var (appToken, userDto) = await _authService.HandleGoogleLoginAsync(payload);

                return Ok(new
                {
                    token = appToken,
                    userDto.FullName,
                    userDto.PhoneNumber,
                    userDto.Email,
                    userDto.Role,
                    hasPassword = !string.IsNullOrEmpty(userDto.PasswordHash)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        // ---------------- Secure test ----------------
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("secure-data")]
        public IActionResult GetSecureData()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            return Ok(new { email, message = "You are authorized!" });
        }

        // ---------------- Đặt mật khẩu cho tài khoản Google ----------------
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("set-password")]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordDto dto)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null) return Unauthorized();

            var success = await _authService.SetPasswordAsync(email, dto.Password);
            if (!success) return NotFound();

            return Ok(new { message = "Mật khẩu đã được lưu." });
        }
    }

    // ---------------- DTO cho Google login ----------------
    public class GoogleLoginDto
    {
        public string IdToken { get; set; } = "";
    }
}
