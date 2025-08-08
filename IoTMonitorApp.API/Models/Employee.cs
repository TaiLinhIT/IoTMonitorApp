using System.ComponentModel.DataAnnotations;

namespace IoTMonitorApp.API.Models
{
    public class Employee : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirtOfDate { get; set; }

    }
}
