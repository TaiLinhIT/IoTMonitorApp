
using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Security.Claims;
using System.Text;

namespace IoTMonitorApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Cấu hình CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost5173", policy =>
                {
                    policy.WithOrigins("http://localhost:5173") // Cho phép React app gọi
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            #region Cấu hình các dịch vụ cần thiết

            // Add services to the container
            builder.Services.AddControllers();
            builder.Services.AddSignalR();
            builder.Services.AddEndpointsApiExplorer();

            #endregion


            // JWT settings
            var jwtKey = builder.Configuration["Authentication:Jwt:Key"];
            var jwtIssuer = builder.Configuration["Authentication:Jwt:Issuer"];
            var jwtAudience = builder.Configuration["Authentication:Jwt:Audience"];
            var clientId = builder.Configuration["Authentication:Google:ClientId"];
            var clientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

            #region Cấu hình xác thực Authentication

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie("Cookies")// Lưu đăng nhập sau khi Google login
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                var jwtKey = builder.Configuration["Authentication:Jwt:Key"];
                var jwtIssuer = builder.Configuration["Authentication:Jwt:Issuer"];
                var jwtAudience = builder.Configuration["Authentication:Jwt:Audience"];
                var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                };
            })
            .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            {
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                options.CallbackPath = "/signin-google";

                options.Events.OnCreatingTicket = async context =>
                {
                    var identifier = context.Identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    // Thêm claim ProviderId vào danh tính
                    if (!string.IsNullOrEmpty(identifier))
                    {
                        var claimsIdentity = (ClaimsIdentity)context.Principal.Identity;
                        claimsIdentity.AddClaim(new Claim("ProviderId", identifier));
                    }
                    //Đăng nhập  cookie khi google trả về thành công
                    await context.HttpContext.SignInAsync(
                        "Cookies", context.Principal);
                    context.Response.Redirect("/api/auth/google-response");
                };

            });
            #endregion
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IBrandService, BrandService>();
            builder.Services.AddScoped<ISpecificationService, SpecificationService>();

            // Đăng ký AutoMapper, tìm tất cả Profiles trong assembly hiện tại
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            #region Cấu hình Entity Framework core

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            #endregion
            #region Cấu hình MongoDb
            builder.Services.Configure<MongoDbSettings>(
                builder.Configuration.GetSection("MongoDbSettings"));

            builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
            {
                var settings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
                return new MongoClient(settings.ConnectionString);
            });

            #endregion


            // Đăng ký Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();
            // 2. Áp dụng CORS
            app.UseCors("AllowLocalhost5173");
            // Chỉ bật swagger ở môi trường development
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();// chuyển hướng HTTP sang HTTPS

            app.UseAuthentication(); // sử dụng xác thực JWT và Google
            app.UseAuthorization(); // sử dụng phân quyền

            app.MapControllers(); // định tuyến cho các controller
                                  //app.MapHub<IoTHub>("/iotHub"); // SignalR hub
            #region Tại sao sử dụng app.MapControllers();
            /*
             * Là để Đăng ký các controller làm endpoints chính cho API
             * vậy tại sao lại là đăng ký các controller làm endpoints chính cho API?
             * Thế Endpoints là gì?
             * Endpoints: là URL mà client dùng để tương tác với hệ thống
             * Controller: là nơi định nghĩa logic cho các endpoint
             * Ở đây thì mỗi method trong controller là một endpoint 
             * có dòng này thì Asp sẽ tìm đến tất cả các controller 
             * và nếu không có dòng này thì Client gọi API sẽ goawpj lõi 404
             
             */
            #endregion
            app.Run();
        }
    }
}

