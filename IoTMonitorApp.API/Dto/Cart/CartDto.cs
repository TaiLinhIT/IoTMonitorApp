namespace IoTMonitorApp.API.Dto.Cart
{
    public class CartDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public List<CartItemDto> Items { get; set; } = new();
        public decimal TotalPrice { get; set; }
    }

}
