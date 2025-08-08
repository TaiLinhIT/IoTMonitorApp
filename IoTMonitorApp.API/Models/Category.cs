using System.ComponentModel.DataAnnotations;

namespace IoTMonitorApp.API.Models
{
    public class Category : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
