using IoTMonitorApp.API.Dto.Cart;
using IoTMonitorApp.API.Models;

namespace IoTMonitorApp.API.IServices
{
    public interface ICartService
    {
        Task<IEnumerable<CartDto>> GetAllAsync();
        Task<CartDto> GetCartByIdAsync(int id);
        Task AddCartAsync(CartDto dto);
        Task<string> UpdateCartAsync(CartDto dto);
        Task<bool> DeleteCartAsync(int id);
        Task<IEnumerable<CartDto>> GetCartsByUserIdAsync(string userId);
        Task<string> AddItemToCartAsync(int cartId, CartItem item);
        Task<string> RemoveItemFromCartAsync(int cartId, int itemId);
    }
}
