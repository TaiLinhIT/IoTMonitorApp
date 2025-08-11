using IoTMonitorApp.API.Dto.PaymentDto;

namespace IoTMonitorApp.API.IServices
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDto>> GetAllAsync();
        Task<PaymentDto> GetByIdAsync(int id);
        Task<PaymentDto> CreateAsync(PaymentDto dto);
        Task<bool> UpdateAsync(PaymentDto dto);
        Task<bool> DeleteAsync(int id);

        // Optional: Các hàm đặc thù cho Payment
        Task<IEnumerable<PaymentDto>> GetByOrderIdAsync(int orderId);
        Task<bool> UpdatePaymentStatusAsync(int paymentId, string status);
    }
}
