using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IoTMonitorApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
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
            var result = await HttpContext.AuthenticateAsync("Google");

            if (!result.Succeeded) return BadRequest();

            var claims = result.Principal.Identities
                .FirstOrDefault()?.Claims.Select(claim => new
                {
                    claim.Type,
                    claim.Value
                });

            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;

            var token = GenerateJwtToken(email);

            return Ok(new
            {
                Email = email,
                Token = token
            });
        }

        private string GenerateJwtToken(string email)
        {
            var jwtSettings = _config.GetSection("Authentication:Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: new[]
                {
                new Claim(ClaimTypes.Email, email)
                },
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
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

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, name ?? ""),
            new Claim(ClaimTypes.Email, email)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Authentication:Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Authentication:Jwt:Issuer"],
                audience: _config["Authentication:Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                email,
                token = tokenStr
            });
        }
    }
}
