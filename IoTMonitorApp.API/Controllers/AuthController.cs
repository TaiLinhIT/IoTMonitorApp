using IoTMonitorApp.API.Dto.Auth.Register;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using IoTMonitorApp.API.Services;
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
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly CsrfService _csrfService;

        public AuthController(IAuthService authService, IJwtService jwtService, CsrfService csrfService)
        {
            _authService = authService;
            _jwtService = jwtService;
            _csrfService = csrfService;
        }

        // ---------------- Refresh token ----------------
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

            // Cập nhật lại cookie refresh token mới (HttpOnly)
            Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // chỉ gửi qua HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = result.Expiry,
            });

            return Ok(new
            {
                accessToken = result.Token,
                csrfToken = result.CsrfToken
            });
        }

        // ---------------- Login với email + password ----------------
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Dto.Auth.Login.CreateLoginDto request)
        {
            try
            {
                var authResult = await _authService.LoginAsync(request);


                // Set refresh token cookie (HttpOnly)
                Response.Cookies.Append("refreshToken", authResult.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, // dev HTTP thì false, deploy HTTPS thì true
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(7)
                });

                return Ok(new
                {
                    accessToken = authResult.Token,
                    csrfToken = authResult.CsrfToken,
                    Role = authResult.UserRole
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        // ---------------- Đăng ký ----------------
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

        // ---------------- Login Google ----------------
        [HttpPost("login-google")]
        public async Task<IActionResult> LoginGoogle([FromBody] GoogleLoginDto dto)
        {
            try
            {
                var payload = await Google.Apis.Auth.GoogleJsonWebSignature.ValidateAsync(dto.IdToken);

                var (accessToken, userDto) = await _authService.HandleGoogleLoginAsync(payload);

                // Sinh refresh token
                var refreshToken = Guid.NewGuid().ToString("N");
                Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7)
                });

                return Ok(new
                {
                    accessToken,
                    user = new
                    {
                        userDto.FullName,
                        userDto.PhoneNumber,
                        userDto.Email,
                        userDto.Role
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ---------------- Logout ----------------
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("refreshToken");
            return Ok(new { message = "Đã đăng xuất." });
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
