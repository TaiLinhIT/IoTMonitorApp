using TypeGen.Core.TypeAnnotations;

namespace IoTMonitorApp.API.Models
{
    [ExportTsClass]
    public class Shipment : BaseEntity
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

    }
}
