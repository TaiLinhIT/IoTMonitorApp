using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Services;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;

namespace IoTMonitorApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. CORS cho FE (http://localhost:5173)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); // ✅ cần cho cookie
                });
            });

            // 2. Controllers
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

            builder.Services.AddSignalR();
            builder.Services.AddEndpointsApiExplorer();

            // 3. JWT settings
            var jwtKey = builder.Configuration["Authentication:Jwt:Key"];
            var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Authentication:Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Authentication:Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("JWT failed: " + context.Exception.Message);
                        return Task.CompletedTask;
                    }
                };
            })
            .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            {
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            });

            // 4. Đăng ký Services (DI)
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IBrandService, BrandService>();
            builder.Services.AddScoped<ISpecificationService, SpecificationService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IShipmentService, ShipmentService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<ICheckoutDraftService, CheckoutDraftService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<IJwtService, JwtService>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // 5. EF Core
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 6. MongoDB
            builder.Services.Configure<MongoDbSettings>(
                builder.Configuration.GetSection("MongoDbSettings"));
            builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
            {
                var settings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
                return new MongoClient(settings.ConnectionString);
            });

            // ❌ 7. Session (KHÔNG CẦN nếu bạn chỉ dùng JWT + cookie refresh token)
            //builder.Services.AddDistributedMemoryCache();
            //builder.Services.AddSession(options =>
            //{
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.IsEssential = true;
            //    options.IdleTimeout = TimeSpan.FromMinutes(30);
            //    options.Cookie.SameSite = SameSiteMode.None;
            //    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
            //});

            // ❌ CookiePolicy (KHÔNG CẦN, vì bạn đã set cookie thủ công trong controller)
            //builder.Services.AddDataProtection();
            //builder.Services.AddCookiePolicy(options =>
            //{
            //    options.CheckConsentNeeded = context => false;
            //});

            // ❌ CSRF (KHÔNG CẦN nếu chỉ dùng JWT, không phải form POST truyền thống)
            //builder.Services.AddAntiforgery(options =>
            //{
            //    options.HeaderName = "X-CSRF-Token";
            //});

            // 8. Swagger
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // --- Pipeline ---
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowFrontend"); // ✅ cookie CORS phải đứng trước Auth

            // ❌ Session không cần
            //app.UseSession();

            // ❌ CookiePolicy không cần
            //app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            // app.MapHub<IoTHub>("/iotHub");

            app.Run();
        }
    }
}
