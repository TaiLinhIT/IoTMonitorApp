using TypeGen.Core.TypeAnnotations;

namespace IoTMonitorApp.API.Models
{
    [ExportTsClass]
    public class Order : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string OrderCode { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public decimal ShippingFee { get; set; }
        public string ShippingMethod { get; set; }
        public string Note { get; set; }

    }
}
