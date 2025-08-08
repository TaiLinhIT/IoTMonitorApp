using System.ComponentModel.DataAnnotations;

namespace IoTMonitorApp.API.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public double PriceAtTime { get; set; }
    }
}
