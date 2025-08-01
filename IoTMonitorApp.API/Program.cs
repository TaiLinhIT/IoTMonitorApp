
using IoTMonitorApp.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitorApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.  

            builder.Services.AddControllers();
            builder.Services.AddSignalR();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.Audience = "IoTMonitorAppAPI";
                    options.RequireHttpsMetadata = true;
                });
            builder.Services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            });
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            var app = builder.Build();

            // Configure the HTTP request pipeline.  
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            //app.MapHub<IoTHub>("/iotHub");//SignalR hub   
            app.Run();
        }
    }
}
