using IoTMonitorApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitorApp.API.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
