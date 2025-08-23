using System.Security.Cryptography;

namespace IoTMonitorApp.API.Middleware
{
    public class CsrfMiddleware
    {
        private readonly RequestDelegate _next;

        public CsrfMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Nếu là request GET -> phát token mới
            if (context.Request.Method == HttpMethods.Get)
            {
                var token = GenerateToken();
                context.Response.Headers["X-CSRF-TOKEN"] = token;
                context.Session.SetString("CSRF-TOKEN", token);
            }
            else
            {
                // Với các request POST, PUT, DELETE -> kiểm tra token
                if (context.Request.Headers.TryGetValue("X-CSRF-TOKEN", out var clientToken))
                {
                    var serverToken = context.Session.GetString("CSRF-TOKEN");

                    if (serverToken == null || clientToken != serverToken)
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("CSRF token is invalid or missing.");
                        return;
                    }
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("CSRF token is required.");
                    return;
                }
            }

            await _next(context);
        }

        private string GenerateToken()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[32];
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }

    // Extension để dễ đăng ký middleware
    public static class CsrfMiddlewareExtensions
    {
        public static IApplicationBuilder UseCsrfMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CsrfMiddleware>();
        }
    }
}

