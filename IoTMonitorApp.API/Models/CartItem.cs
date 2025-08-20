using System.ComponentModel.DataAnnotations;
using TypeGen.Core.TypeAnnotations;

namespace IoTMonitorApp.API.Models
{
    [ExportTsClass]
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public Guid ProductId { get; set; }
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public double PriceAtTime { get; set; }
    }
}
