using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.Dto.Cart;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitorApp.API.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _dbContext;
        public CartService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddCartAsync(CartDto dto)
        {
            var cart = new Cart
            {
                UserId = dto.UserId,
                IsCheckOut = dto.IsCheckOut,
                Slug = dto.Slug,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = null,
                IsDelete = false
            };

            await _dbContext.carts.AddAsync(cart);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<CartDto>> GetAllAsync()
        {
            return await _dbContext.carts
                .Select(c => new CartDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    IsCheckOut = c.IsCheckOut,
                    Slug = c.Slug,
                    CreatedDate = c.CreatedDate,
                    UpdatedDate = c.UpdatedDate,
                    IsDelete = c.IsDelete
                })
                .ToListAsync();
        }

        public async Task<CartDto> GetCartByIdAsync(int id)
        {
            var cart = await _dbContext.carts.FirstOrDefaultAsync(c => c.Id == id);
            if (cart == null) return null;

            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                IsCheckOut = cart.IsCheckOut,
                Slug = cart.Slug,
                CreatedDate = cart.CreatedDate,
                UpdatedDate = cart.UpdatedDate,
                IsDelete = cart.IsDelete
            };
        }

        public async Task<string> UpdateCartAsync(CartDto dto)
        {
            var cart = await _dbContext.carts.FirstOrDefaultAsync(c => c.Id == dto.Id);
            if (cart == null) return "Cart not found";

            cart.IsCheckOut = dto.IsCheckOut;
            cart.Slug = dto.Slug;
            cart.UpdatedDate = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            return "Update successful";
        }

        public async Task<bool> DeleteCartAsync(int id)
        {
            var cart = await _dbContext.carts.FirstOrDefaultAsync(c => c.Id == id);
            if (cart == null) return false;
            cart.IsDelete = true;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CartDto>> GetCartsByUserIdAsync(string userId)
        {
            try
            {
                var carts = await _dbContext.carts
                    .Where(c => c.UserId.ToString() == userId && !c.IsDelete)
                    .Select(c => new CartDto
                    {
                        Id = c.Id,
                        UserId = c.UserId,
                        IsCheckOut = c.IsCheckOut,
                        Slug = c.Slug,
                        CreatedDate = c.CreatedDate,
                        UpdatedDate = c.UpdatedDate,
                        IsDelete = c.IsDelete
                    })
                    .ToListAsync();

                return carts;
            }
            catch (Exception ex)
            {
                // Có thể log lỗi tại đây
                throw new Exception($"Error retrieving carts for user {userId}: {ex.Message}");
            }
        }


        public async Task<string> AddItemToCartAsync(int cartId, CartItem item)
        {
            try
            {
                var cartExists = await _dbContext.carts.AnyAsync(c => c.Id == cartId);
                if (!cartExists)
                    return "Cart not found";

                var existingItem = await _dbContext.CartItems
                    .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == item.ProductId);

                if (existingItem != null)
                {
                    existingItem.Quantity += item.Quantity;
                }
                else
                {
                    item.CartId = cartId;
                    await _dbContext.CartItems.AddAsync(item);
                }

                await _dbContext.SaveChangesAsync();
                return "Item added successfully";
            }
            catch (Exception ex)
            {
                return $"Error adding item: {ex.Message}";
            }
        }

        public Task<string> RemoveItemFromCartAsync(int cartId, int itemId)
        {
            throw new NotImplementedException();
        }
    }
}
