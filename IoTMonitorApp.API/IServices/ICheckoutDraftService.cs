using IoTMonitorApp.API.Dto.Checkout;
using IoTMonitorApp.API.Models;

namespace IoTMonitorApp.API.IServices
{
    public interface ICheckoutDraftService
    {
        Task<CheckoutDraft> CreateDraftAsync(CheckoutDraftCreateDto dto, Guid UserId);

    }
}
