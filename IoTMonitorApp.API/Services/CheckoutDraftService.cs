using IoTMonitorApp.API.Data; // Giả sử MyDbContext
using IoTMonitorApp.API.Dto.Checkout;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitorApp.API.Services
{
    public class CheckoutDraftService : ICheckoutDraftService
    {
        private readonly AppDbContext _context;

        public CheckoutDraftService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CheckoutDraft> CreateDraftAsync(CheckoutDraftCreateDto dto, Guid UserId)
        {
            // 1. Tạo draft
            var draft = new CheckoutDraft
            {
                UserId = UserId,
                TotalPrice = dto.TotalPrice,
                Status = "Draft",
                CreatedAt = DateTime.UtcNow
            };

            _context.CheckoutDrafts.Add(draft);
            await _context.SaveChangesAsync();

            // 2. Lưu từng item
            foreach (var itemDto in dto.Items)
            {
                var draftItem = new CheckoutDraftItem
                {
                    CheckoutDraftId = draft.Id,
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    Price = itemDto.Price
                };
                _context.CheckoutDraftItems.Add(draftItem);
            }
            var cartItems = _context.CartItems.Where(ci => dto.Items.Select(i => i.ProductId).Contains(ci.ProductId));
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();


            return draft;
        }

        public async Task<CheckoutDraft> GetDraftByIdAsync(int draftId)
        {
            return await _context.CheckoutDrafts.FirstOrDefaultAsync(d => d.Id == draftId);
        }
    }
}
