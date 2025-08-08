using System.ComponentModel.DataAnnotations;

namespace IoTMonitorApp.API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        public DateTime AssignedDate { get; set; }
        public string? GoogleId { get; set; }
        [Required]
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDelete { get; set; }
    }
}
