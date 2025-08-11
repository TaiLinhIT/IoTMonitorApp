namespace IoTMonitorApp.API.Dto.Cart
{
    public class CartDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsCheckOut { get; set; }
        public string? Slug { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDelete { get; set; } = false;
    }
}
