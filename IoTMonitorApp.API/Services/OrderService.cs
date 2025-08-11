using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.Dto.Order;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitorApp.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _dbContext;

        public OrderService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var orders = await _dbContext.Orders.AsNoTracking().ToListAsync();
            return orders.Select(o => MapToDto(o));
        }

        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var order = await _dbContext.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);
            return order == null ? null : MapToDto(order);
        }

        public async Task<OrderDto> CreateAsync(OrderDto dto)
        {
            var order = MapToEntity(dto);
            order.CreatedDate = DateTime.UtcNow;

            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            return MapToDto(order);
        }

        public async Task<bool> UpdateAsync(OrderDto dto)
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == dto.Id);
            if (order == null) return false;

            // Update fields
            order.UserId = dto.UserId;
            order.OrderCode = dto.OrderCode;
            order.OrderDate = dto.OrderDate;
            order.Status = dto.Status;
            order.PaymentMethod = dto.PaymentMethod;
            order.PaymentStatus = dto.PaymentStatus;
            order.TotalAmount = dto.TotalAmount;
            order.ShippingAddress = dto.ShippingAddress;
            order.ShippingFee = dto.ShippingFee;
            order.ShippingMethod = dto.ShippingMethod;
            order.Note = dto.Note;
            order.Slug = dto.Slug;
            order.UpdatedDate = DateTime.UtcNow;
            order.IsDelete = dto.IsDelete;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return false;

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        // Helper: Map Entity -> DTO
        private static OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderCode = order.OrderCode,
                OrderDate = order.OrderDate,
                Status = order.Status,
                PaymentMethod = order.PaymentMethod,
                PaymentStatus = order.PaymentStatus,
                TotalAmount = order.TotalAmount,
                ShippingAddress = order.ShippingAddress,
                ShippingFee = order.ShippingFee,
                ShippingMethod = order.ShippingMethod,
                Note = order.Note,
                Slug = order.Slug,
                CreatedDate = order.CreatedDate,
                UpdatedDate = order.UpdatedDate,
                IsDelete = order.IsDelete
            };
        }

        // Helper: Map DTO -> Entity
        private static Order MapToEntity(OrderDto dto)
        {
            return new Order
            {
                Id = dto.Id,
                UserId = dto.UserId,
                OrderCode = dto.OrderCode,
                OrderDate = dto.OrderDate,
                Status = dto.Status,
                PaymentMethod = dto.PaymentMethod,
                PaymentStatus = dto.PaymentStatus,
                TotalAmount = dto.TotalAmount,
                ShippingAddress = dto.ShippingAddress,
                ShippingFee = dto.ShippingFee,
                ShippingMethod = dto.ShippingMethod,
                Note = dto.Note,
                Slug = dto.Slug,
                CreatedDate = dto.CreatedDate,
                UpdatedDate = dto.UpdatedDate,
                IsDelete = dto.IsDelete
            };
        }
    }
}
