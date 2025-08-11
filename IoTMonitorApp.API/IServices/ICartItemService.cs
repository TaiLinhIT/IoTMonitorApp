using IoTMonitorApp.API.Dto.CartItem;

namespace IoTMonitorApp.API.IServices
{
    public interface ICartItemService
    {
        Task<IEnumerable<CartItemDto>> GetAllAsync();
        Task<CartItemDto?> GetByIdAsync(int id);
        Task<CartItemDto> CreateAsync(CartItemDto dto);
        Task<bool> UpdateAsync(CartItemDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
