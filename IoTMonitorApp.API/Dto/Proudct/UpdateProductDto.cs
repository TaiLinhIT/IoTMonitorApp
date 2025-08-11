namespace IoTMonitorApp.API.Dto.Proudct
{
    public class UpdateProductDto
    {
        public Guid Id { get; set; }  // Dùng Guid để xác định sản phẩm cần update
        public string Name { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public int SpecificationsId { get; set; }
    }
}
