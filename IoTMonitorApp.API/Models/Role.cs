using System.ComponentModel.DataAnnotations;

namespace IoTMonitorApp.API.Models
{
    public class Role : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

    }
}
