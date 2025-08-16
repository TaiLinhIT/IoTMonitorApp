namespace IoTMonitorApp.API.Dto.Proudct
{
    public class ProductItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Slug { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public string SpecificationsName { get; set; }
        public decimal Price { get; set; }
        public List<string> ProductUrl { get; set; }

    }
}
