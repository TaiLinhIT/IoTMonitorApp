

using IoTMonitorApp.API.Dto.Order;

namespace IoTMonitorApp.API.IServices
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItemDto>> GetAllAsync();
        Task<OrderItemDto?> GetByIdAsync(int id);
        Task<OrderItemDto> CreateAsync(OrderItemDto dto);
        Task<bool> UpdateAsync(OrderItemDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
