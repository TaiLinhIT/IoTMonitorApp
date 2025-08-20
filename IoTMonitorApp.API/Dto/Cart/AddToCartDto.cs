namespace IoTMonitorApp.API.Dto.Cart
{
    public class AddToCartDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
