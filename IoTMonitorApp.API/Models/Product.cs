using TypeGen.Core.TypeAnnotations;

namespace IoTMonitorApp.API.Models
{
    [ExportTsClass]
    public class Product : BaseEntity
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public int CartId { get; set; }
        public int SpecificationsId { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
