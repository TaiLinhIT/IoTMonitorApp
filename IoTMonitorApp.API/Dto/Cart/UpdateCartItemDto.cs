namespace IoTMonitorApp.API.Dto.Cart
{
    public class UpdateCartItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
