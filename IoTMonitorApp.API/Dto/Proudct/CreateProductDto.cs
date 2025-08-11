namespace IoTMonitorApp.API.Dto.Proudct
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public int SpecificationsId { get; set; }
    }
}
