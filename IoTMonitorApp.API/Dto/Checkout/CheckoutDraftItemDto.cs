namespace IoTMonitorApp.API.Dto.Checkout
{
    public class CheckoutDraftItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
