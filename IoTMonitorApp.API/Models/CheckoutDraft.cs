using System.ComponentModel.DataAnnotations;

namespace IoTMonitorApp.API.Models
{
    public class CheckoutDraft
    {
        [Key]
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = "Draft";
    }
}
