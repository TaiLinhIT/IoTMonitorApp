using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.Dto.Cart;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitorApp.API.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CartDto?> GetCartByUserAsync(Guid userId)
        {
            var cart = await _context.carts
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsCheckOut);

            if (cart == null) return null;

            var items = await _context.CartItems
                .Where(i => i.CartId == cart.Id)
                .ToListAsync();

            // lấy thêm thông tin product (nếu cần hiển thị)
            var productIds = items.Select(i => i.ProductId).ToList();
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            var itemDtos = items.Select(i =>
            {
                var product = products.FirstOrDefault(p => p.Id == i.ProductId);
                return new CartItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = product?.Name ?? "",
                    ImageUrl = product?.ProductUrl.FirstOrDefault(),
                    Price = (decimal)i.PriceAtTime,
                    Quantity = i.Quantity
                };
            }).ToList();

            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = itemDtos,
                TotalPrice = itemDtos.Sum(i => i.Price * i.Quantity)
            };
        }

        public async Task<CartDto> AddItemAsync(Guid userId, Guid productId, int quantity)
        {
            var cart = await _context.carts
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsCheckOut);

            if (cart == null)
            {
                cart = new Cart { UserId = userId, IsCheckOut = false, CreatedDate = DateTime.UtcNow };
                _context.carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(i => i.CartId == cart.Id && i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null) throw new Exception("Product not found");

                var newItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity,
                    PriceAtTime = (double)product.Price
                };
                _context.CartItems.Add(newItem);
            }

            await _context.SaveChangesAsync();
            return await GetCartByUserAsync(userId) ?? throw new Exception("Cart error");
        }

        public async Task<CartDto> UpdateItemAsync(Guid userId, Guid productId, int quantity)
        {
            var cart = await _context.carts
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsCheckOut);

            if (cart == null) throw new Exception("Cart not found");

            var item = await _context.CartItems
                .FirstOrDefaultAsync(i => i.CartId == cart.Id && i.ProductId == productId);

            if (item == null) throw new Exception("Item not found");

            item.Quantity = quantity;
            await _context.SaveChangesAsync();

            return await GetCartByUserAsync(userId) ?? throw new Exception("Cart error");
        }

        public async Task<CartDto> RemoveItemAsync(Guid userId, Guid productId)
        {
            var cart = await _context.carts
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsCheckOut);

            if (cart == null) throw new Exception("Cart not found");

            var item = await _context.CartItems
                .FirstOrDefaultAsync(i => i.CartId == cart.Id && i.ProductId == productId);

            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }

            return await GetCartByUserAsync(userId) ?? throw new Exception("Cart error");
        }

        public async Task<bool> ClearCartAsync(Guid userId)
        {
            var cart = await _context.carts
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsCheckOut);

            if (cart == null) return false;

            var items = _context.CartItems.Where(i => i.CartId == cart.Id);
            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();

            return true;
        }
    }

}
