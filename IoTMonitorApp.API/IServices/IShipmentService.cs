using IoTMonitorApp.API.Dto.Shipment;

namespace IoTMonitorApp.API.IServices
{
    public interface IShipmentService
    {
        Task<IEnumerable<ShipmentDto>> GetAllAsync();
        Task<ShipmentDto?> GetByIdAsync(int id);
        Task<IEnumerable<ShipmentDto>> GetByOrderIdAsync(int orderId);
        Task<ShipmentDto> CreateAsync(ShipmentDto dto);
        Task<bool> UpdateAsync(ShipmentDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateShipmentStatusAsync(int id, string status);
    }
}
