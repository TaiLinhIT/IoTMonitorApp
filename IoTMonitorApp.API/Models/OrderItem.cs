using System.ComponentModel.DataAnnotations;
using TypeGen.Core.TypeAnnotations;

namespace IoTMonitorApp.API.Models
{
    [ExportTsClass]
    public class OrderItem : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
