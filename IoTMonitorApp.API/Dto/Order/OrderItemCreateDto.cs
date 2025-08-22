namespace IoTMonitorApp.API.Dto.Order
{
    public class OrderItemCreateDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
