using IoTMonitorApp.API.Dto.Auth.Login;
using IoTMonitorApp.API.Dto.Auth.Register;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.AspNetCore.Authentication;
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
        public AuthController(IConfiguration config, IAuthService authService)
        {
            _config = config;
            _authService = authService;
        }

        [HttpGet("login-google")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, "Google");
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync("Cookies");
            if (!result.Succeeded) return Unauthorized();

            try
            {
                var (token, user) = await _authService.HandleGoogleLoginAsync(result.Principal);
                return Ok(new
                {
                    user.Email,
                    user.FullName,
                    token,
                    hasPassword = !string.IsNullOrEmpty(user.PasswordHash) // <== kiểm tra đã có mật khẩu chưa
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("secure-data")]
        public IActionResult GetSecureData()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            return Ok(new { email, message = "You are authorized!" });
        }


        [HttpGet("callback")]
        public IActionResult Callback()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var name = User.FindFirst(ClaimTypes.Name)?.Value;

            if (email == null)
            {
                return BadRequest("Không lấy được thông tin email");
            }

            var token = _authService.GenerateJwtToken(email);

            return Ok(new { email, name, token });
        }
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
    }
}