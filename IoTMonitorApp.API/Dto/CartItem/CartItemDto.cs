﻿namespace IoTMonitorApp.API.Dto.CartItem
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public double PriceAtTime { get; set; }
    }
}
