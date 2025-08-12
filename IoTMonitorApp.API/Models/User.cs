using System.ComponentModel.DataAnnotations;
using TypeGen.Core.TypeAnnotations;

namespace IoTMonitorApp.API.Models
{
    [ExportTsClass]
    public class User : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int AddressId { get; set; }
        public DateTime BirthOfDate { get; set; }
        public DateTime AssignedDate { get; set; }
        public string? GoogleId { get; set; }
        public string? Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public string? ImageUrl { get; set; }

    }
}
