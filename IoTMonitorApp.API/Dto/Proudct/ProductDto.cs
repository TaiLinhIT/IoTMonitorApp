namespace IoTMonitorApp.API.Dto.Proudct
{
    public class ProductDto
    {
        public Guid Id { get; set; }        // Nếu là API trả dữ liệu thì giữ Id
        public string Name { get; set; }
        public string? Slug { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public string SpecificationsName { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
