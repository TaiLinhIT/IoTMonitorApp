using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitorApp.API.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly AppDbContext _context;

        public CheckoutService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CheckoutDraft> CreateDraftAsync(Guid userId, List<CheckoutDraftItem> items, decimal totalPrice)
        {
            var draft = new CheckoutDraft
            {
                UserId = userId,
                TotalPrice = totalPrice,
                CreatedAt = DateTime.UtcNow,
                Status = "Draft"
            };

            await _context.CheckoutDrafts.AddAsync(draft);
            await _context.SaveChangesAsync();

            foreach (var item in items)
            {
                item.CheckoutDraftId = draft.Id;
            }

            await _context.CheckoutDraftItems.AddRangeAsync(items);
            await _context.SaveChangesAsync();

            return draft;
        }

        public async Task<CheckoutDraft> GetDraftByIdAsync(int draftId)
        {
            return await _context.CheckoutDrafts.FindAsync(draftId);
        }

        public async Task<List<CheckoutDraftItem>> GetDraftItemsAsync(int draftId)
        {
            return await _context.CheckoutDraftItems
                                 .Where(i => i.CheckoutDraftId == draftId)
                                 .ToListAsync();
        }

        public async Task<bool> ConfirmDraftAsync(int draftId)
        {
            var draft = await _context.CheckoutDrafts.FindAsync(draftId);
            if (draft == null) return false;

            draft.Status = "Confirmed";
            _context.CheckoutDrafts.Update(draft);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
