using IoTMonitorApp.API.Models;

namespace IoTMonitorApp.API.IServices
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetAllAsync();
        Task<Cart> GetCartByIdAsync(int id);
        Task AddCartAsync(Cart cart);
        Task<string> UpdateCartAsync(Cart cart);
        Task<bool> DeleteCartAsync(int id);
        Task<IEnumerable<Cart>> GetCartsByUserIdAsync(string userId);
        Task<string> AddItemToCartAsync(int cartId, CartItem item);
        Task<string> RemoveItemFromCartAsync(int cartId, int itemId);
    }
}
