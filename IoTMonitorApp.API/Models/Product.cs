namespace IoTMonitorApp.API.Models
{
    public class Product : BaseEntity
    {
        public Guid Id { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public int CartId { get; set; }
        public int SpecificationsId { get; set; }
        public string Name { get; set; }
    }
}
