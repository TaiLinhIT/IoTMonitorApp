using IoTMonitorApp.API.Dto.Order;

namespace IoTMonitorApp.API.IServices
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task<OrderDto?> GetByIdAsync(int id);
        Task<OrderDto> CreateAsync(OrderCreateDto dto);
        Task<bool> UpdateAsync(OrderDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
