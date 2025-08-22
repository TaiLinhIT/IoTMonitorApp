using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.Dto.OrderItem;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitorApp.API.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly AppDbContext _dbContext;

        public OrderItemService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<OrderItemDto>> GetAllAsync()
        {
            var items = await _dbContext.OrderItems.ToListAsync();
            return items.Select(MapToDto);
        }

        public async Task<OrderItemDto?> GetByIdAsync(int id)
        {
            var item = await _dbContext.OrderItems.FindAsync(id);
            return item == null ? null : MapToDto(item);
        }

        public async Task<OrderItemDto> CreateAsync(OrderItemDto dto)
        {
            var entity = MapToEntity(dto);
            entity.CreatedDate = DateTime.UtcNow;

            _dbContext.OrderItems.Add(entity);
            await _dbContext.SaveChangesAsync();

            return MapToDto(entity);
        }

        public async Task<bool> UpdateAsync(OrderItemDto dto)
        {
            var existing = await _dbContext.OrderItems.FindAsync(dto.Id);
            if (existing == null) return false;

            existing.ProductId = dto.ProductId;
            existing.ProductName = dto.ProductName;
            existing.Quantity = dto.Quantity;
            existing.UnitPrice = dto.UnitPrice;
            existing.Discount = dto.Discount;
            existing.TotalPrice = dto.TotalPrice;
            existing.UpdatedDate = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _dbContext.OrderItems.FindAsync(id);
            if (existing == null) return false;

            _dbContext.OrderItems.Remove(existing);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        // ================== Helper mapping ==================

        private static OrderItemDto MapToDto(OrderItem entity)
        {
            return new OrderItemDto
            {
                Id = entity.Id,
                ProductId = entity.ProductId,
                ProductName = entity.ProductName,
                Quantity = entity.Quantity,
                UnitPrice = entity.UnitPrice,
                Discount = entity.Discount,
                TotalPrice = entity.TotalPrice,
                OrderId = entity.OrderId,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate
            };
        }

        private static OrderItem MapToEntity(OrderItemDto dto)
        {
            return new OrderItem
            {
                Id = dto.Id,
                ProductId = dto.ProductId,
                ProductName = dto.ProductName,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice,
                Discount = dto.Discount,
                TotalPrice = dto.TotalPrice,
                OrderId = dto.OrderId,
                CreatedDate = dto.CreatedDate,
                UpdatedDate = dto.UpdatedDate
            };
        }
    }
}
