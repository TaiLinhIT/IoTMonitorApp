namespace IoTMonitorApp.API.Dto.Order
{
    public class OrderCreateDto
    {
        public int UserId { get; set; }
        public List<OrderItemCreateDto> Items { get; set; }
    }
}
