namespace IoTMonitorApp.API.Dto.Order
{
    public class OrderDto
    {
        //public int Id { get; set; }
        //public int UserId { get; set; }
        //public string OrderCode { get; set; }
        //public DateTime OrderDate { get; set; }
        //public string Status { get; set; }
        //public string PaymentMethod { get; set; }
        //public string PaymentStatus { get; set; }
        //public decimal TotalAmount { get; set; }
        //public string ShippingAddress { get; set; }
        //public decimal ShippingFee { get; set; }
        //public string ShippingMethod { get; set; }
        //public string Note { get; set; }
        //public string? Slug { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime? UpdatedDate { get; set; }
        //public bool IsDelete { get; set; } = false;
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
