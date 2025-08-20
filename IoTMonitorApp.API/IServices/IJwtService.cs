using IoTMonitorApp.API.Models;
using System.Security.Claims;

namespace IoTMonitorApp.API.IServices
{
    public interface IJwtService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal? ValidateToken(string token);
        string GenerateJwtToken(User user);
    }
}
