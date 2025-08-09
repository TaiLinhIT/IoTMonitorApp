namespace IoTMonitorApp.API.Models
{
    public abstract class BaseEntity
    {
        public string? Slug { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDelete { get; set; }

    }
}
