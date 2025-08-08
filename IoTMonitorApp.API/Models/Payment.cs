namespace IoTMonitorApp.API.Models
{
    public class Payment : BaseEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? TransactionId { get; set; }
        public string Status { get; set; }

    }
}
