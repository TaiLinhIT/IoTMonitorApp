using System.ComponentModel.DataAnnotations;
using TypeGen.Core.TypeAnnotations;

namespace IoTMonitorApp.API.Models
{
    [ExportTsClass]
    public class User : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public int RoleId { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

        public DateTime? BirthOfDate { get; set; }
        public DateTime? AssignedDate { get; set; }
        public string? GoogleId { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public string? ImageUrl { get; set; }

    }
}
