using System.ComponentModel.DataAnnotations;

namespace IoTMonitorApp.API.Models
{
    public class Cart : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsCheckOut { get; set; }
    }
}
