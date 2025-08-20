using Google.Apis.Auth; // ✅ cần package Google.Apis.Auth
using IoTMonitorApp.API.Dto.Auth.Login;
using IoTMonitorApp.API.Dto.Auth.Register;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IoTMonitorApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;

        public AuthController(IConfiguration config, IAuthService authService, IJwtService jwtService)
        {
            _config = config;
            _authService = authService;
            _jwtService = jwtService;
        }

        // ---------------- Email + Password ----------------
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CreateLoginDto dto)
        {
            try
            {
                var token = await _authService.LoginAsync(dto);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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
                // ✅ Verify id_token từ Google
                var payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _config["Google:ClientId"] }
                });

                // ✅ gọi service với payload
                var (token, user) = await _authService.HandleGoogleLoginAsync(payload);

                return Ok(new
                {
                    user.Email,
                    user.FullName,
                    Token = token,
                    hasPassword = !string.IsNullOrEmpty(user.PasswordHash)
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = "Xác thực Google thất bại", error = ex.Message });
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
