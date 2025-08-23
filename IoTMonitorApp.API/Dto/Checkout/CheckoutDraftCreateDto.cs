namespace IoTMonitorApp.API.Dto.Checkout
{
    public class CheckoutDraftCreateDto
    {
        public decimal TotalPrice { get; set; }
        public List<CheckoutDraftItemDto> Items { get; set; }
    }
}
