using IoTMonitorApp.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace IoTMonitorApp.API.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Specification> Specifications { get; set; }
        public DbSet<CheckoutDraft> CheckoutDrafts { get; set; }
        public DbSet<CheckoutDraftItem> CheckoutDraftItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.ProductUrl)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),   // List<string> -> string
                    v => string.IsNullOrEmpty(v) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) // string -> List<string>
                );
        }

    }
}
