using IoTMonitorApp.API.Models;

namespace IoTMonitorApp.API.IServices
{
    public interface ICheckoutService
    {
        Task<CheckoutDraft> CreateDraftAsync(Guid userId, List<CheckoutDraftItem> items, decimal totalPrice);
        Task<CheckoutDraft> GetDraftByIdAsync(int draftId);
        Task<List<CheckoutDraftItem>> GetDraftItemsAsync(Guid draftId);
        Task<bool> ConfirmDraftAsync(int draftId); // ví dụ confirm thanh toán
        Task<List<CheckoutDraftItem>> GetDraftByUserId(Guid userId);
    }
}
