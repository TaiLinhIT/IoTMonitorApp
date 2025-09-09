namespace IoTMonitorApp.API.Models
{
    public class CheckoutDraftItem
    {
        public int Id { get; set; }
        public Guid CheckoutDraftId { get; set; } // liên kết thủ công với CheckoutDraft
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

    }
}
