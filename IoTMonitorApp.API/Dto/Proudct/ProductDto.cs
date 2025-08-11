namespace IoTMonitorApp.API.Dto.Proudct
{
    public class ProductDto
    {
        public Guid Id { get; set; }        // Nếu là API trả dữ liệu thì giữ Id
        public string Name { get; set; }
        public string? Slug { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public int SpecificationsId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
