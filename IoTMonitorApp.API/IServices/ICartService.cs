using IoTMonitorApp.API.Dto.Cart;

namespace IoTMonitorApp.API.IServices
{
    public interface ICartService
    {
        Task<CartDto?> GetCartByUserAsync(Guid userId);
        Task<CartDto> AddItemAsync(Guid userId, Guid productId, int quantity);
        Task<CartDto> UpdateItemAsync(Guid userId, Guid productId, int quantity);
        Task<CartDto> RemoveItemAsync(Guid userId, Guid productId);
        Task<bool> ClearCartAsync(Guid userId);
    }

}
