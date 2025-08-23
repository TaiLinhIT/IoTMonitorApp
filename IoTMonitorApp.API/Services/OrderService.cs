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



        public async Task<bool> UpdateAsync(OrderDto dto)
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == dto.Id);
            if (order == null) return false;

            // Update fields
            order.UserId = dto.UserId;
            order.UpdatedDate = DateTime.UtcNow;

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
            };
        }



        public async Task<OrderDto> CreateAsync(OrderCreateDto dto)
        {
            try
            {
                // 1. Tạo mới Order
                var order = new Order
                {
                    UserId = dto.UserId,
                    CreatedDate = DateTime.UtcNow,
                    TotalAmount = 0 // sẽ tính sau
                };

                _dbContext.Orders.Add(order);
                await _dbContext.SaveChangesAsync(); // Lưu trước để có OrderId

                decimal totalAmount = 0;

                // 2. Thêm OrderItems từ DTO
                foreach (var item in dto.Items)
                {
                    // Lấy giá sản phẩm từ DB
                    var product = await _dbContext.Products.FindAsync(item.ProductId);
                    if (product == null)
                        throw new Exception($"Product {item.ProductId} not found");

                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price
                    };

                    totalAmount += product.Price * item.Quantity;

                    _dbContext.OrderItems.Add(orderItem);
                }

                // 3. Cập nhật tổng tiền
                order.TotalAmount = totalAmount;
                _dbContext.Orders.Update(order);
                await _dbContext.SaveChangesAsync();

                // 4. Map sang DTO trả về
                var orderDto = new OrderDto
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    TotalAmount = order.TotalAmount,
                    CreatedDate = order.CreatedDate,
                    Items = dto.Items.Select(x => new OrderItemDto
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        UnitPrice = _dbContext.Products.First(p => p.Id == x.ProductId).Price
                    }).ToList()
                };

                return orderDto;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return null; // Hoặc ném ngoại lệ tùy ý
            }

        }

    }
}
