namespace IoTMonitorApp.API.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiryDate { get; set; }  // hạn sử dụng
        public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
        public bool IsRevoked { get; set; }
        // Khóa ngoại
        public Guid UserId { get; set; }

        // Navigation property
        public User User { get; set; }
    }

}
