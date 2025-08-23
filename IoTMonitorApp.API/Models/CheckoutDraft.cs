namespace IoTMonitorApp.API.Models
{
    public class CheckoutDraft
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = "Draft";
    }
}
