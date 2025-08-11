using AutoMapper;
using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.Dto.Shipment;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitorApp.API.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public ShipmentService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ShipmentDto>> GetAllAsync()
        {
            var shipments = await _dbContext.Shipments
                .Where(s => !s.IsDelete)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ShipmentDto>>(shipments);
        }

        public async Task<ShipmentDto?> GetByIdAsync(int id)
        {
            var shipment = await _dbContext.Shipments
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDelete);
            return shipment == null ? null : _mapper.Map<ShipmentDto>(shipment);
        }

        public async Task<IEnumerable<ShipmentDto>> GetByOrderIdAsync(int orderId)
        {
            var shipments = await _dbContext.Shipments
                .Where(s => s.OrderId == orderId && !s.IsDelete)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ShipmentDto>>(shipments);
        }

        public async Task<ShipmentDto> CreateAsync(ShipmentDto dto)
        {
            var shipment = _mapper.Map<Shipment>(dto);
            shipment.CreatedDate = DateTime.UtcNow;

            _dbContext.Shipments.Add(shipment);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ShipmentDto>(shipment);
        }

        public async Task<bool> UpdateAsync(ShipmentDto dto)
        {
            var shipment = await _dbContext.Shipments
                .FirstOrDefaultAsync(s => s.Id == dto.Id && !s.IsDelete);

            if (shipment == null) return false;

            _mapper.Map(dto, shipment);
            shipment.UpdatedDate = DateTime.UtcNow;

            _dbContext.Shipments.Update(shipment);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var shipment = await _dbContext.Shipments
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDelete);

            if (shipment == null) return false;

            shipment.IsDelete = true;
            shipment.UpdatedDate = DateTime.UtcNow;

            _dbContext.Shipments.Update(shipment);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateShipmentStatusAsync(int id, string status)
        {
            var shipment = await _dbContext.Shipments
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDelete);

            if (shipment == null) return false;

            shipment.Status = status;
            shipment.UpdatedDate = DateTime.UtcNow;

            _dbContext.Shipments.Update(shipment);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
