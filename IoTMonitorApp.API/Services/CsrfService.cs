using System.Security.Cryptography;

namespace IoTMonitorApp.API.Services
{
    public class CsrfService
    {
        public string GenerateToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(bytes);
        }
    }
}
