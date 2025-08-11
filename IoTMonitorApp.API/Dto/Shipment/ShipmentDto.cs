namespace IoTMonitorApp.API.Dto.Shipment
{
    public class ShipmentDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string ShipmentMethod { get; set; }
        public string Carrier { get; set; }
        public string TrackingNumber { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; }
        public decimal ShippingFee { get; set; }
        public string? Slug { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDelete { get; set; } = false;
    }
}
