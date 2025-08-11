using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.Dto.CartItem;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitorApp.API.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly AppDbContext _dbContext;
        public CartItemService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CartItemDto> CreateAsync(CartItemDto dto)
        {
            var entity = new CartItem
            {
                ProductId = dto.ProductId,
                CartId = dto.CartId,
                Quantity = dto.Quantity,
                PriceAtTime = dto.PriceAtTime
            };

            await _dbContext.CartItems.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            dto.Id = entity.Id; // cập nhật lại ID sau khi tạo
            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbContext.CartItems.FindAsync(id);
            if (entity == null) return false;

            _dbContext.CartItems.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CartItemDto>> GetAllAsync()
        {
            return await _dbContext.CartItems
                .Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    CartId = ci.CartId,
                    Quantity = ci.Quantity,
                    PriceAtTime = ci.PriceAtTime
                }).ToListAsync();
        }

        public async Task<CartItemDto?> GetByIdAsync(int id)
        {
            var ci = await _dbContext.CartItems.FindAsync(id);
            if (ci == null) return null;

            return new CartItemDto
            {
                Id = ci.Id,
                ProductId = ci.ProductId,
                CartId = ci.CartId,
                Quantity = ci.Quantity,
                PriceAtTime = ci.PriceAtTime
            };
        }

        public async Task<bool> UpdateAsync(CartItemDto dto)
        {
            var entity = await _dbContext.CartItems.FindAsync(dto.Id);
            if (entity == null) return false;

            entity.ProductId = dto.ProductId;
            entity.CartId = dto.CartId;
            entity.Quantity = dto.Quantity;
            entity.PriceAtTime = dto.PriceAtTime;

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
