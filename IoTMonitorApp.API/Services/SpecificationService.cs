using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.Dto.Specification;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitorApp.API.Services
{
    public class SpecificationService : ISpecificationService
    {
        private readonly AppDbContext _context;

        public SpecificationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SpecificationDto>> GetAllAsync()
        {
            var entities = await _context.Specifications
                .AsNoTracking()
                .Where(x => !x.IsDelete)
                .ToListAsync();

            return entities.Select(e => new SpecificationDto
            {
                Id = e.Id,
                Storage = e.Storage,
                SizeDisplay = e.SizeDisplay,
                Color = e.Color,
                Material = e.Material,
                Battery = e.Battery
            });
        }

        public async Task<SpecificationDto> GetSpecificationByIdAsync(int id)
        {
            var e = await _context.Specifications
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDelete);

            if (e == null) return null;

            return new SpecificationDto
            {
                Id = e.Id,
                Storage = e.Storage,
                SizeDisplay = e.SizeDisplay,
                Color = e.Color,
                Material = e.Material,
                Battery = e.Battery
            };
        }

        public async Task AddSpecificationAsync(SpecificationDto dto)
        {
            var entity = new Specification
            {
                Storage = dto.Storage,
                SizeDisplay = dto.SizeDisplay,
                Color = dto.Color,
                Material = dto.Material,
                Battery = dto.Battery,
                CreatedDate = DateTime.UtcNow,
                IsDelete = false
            };

            _context.Specifications.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<string> UpdateSpecificationAsync(SpecificationDto dto)
        {
            var entity = await _context.Specifications.FindAsync(dto.Id);
            if (entity == null || entity.IsDelete)
            {
                return "Specification not found.";
            }

            entity.Storage = dto.Storage;
            entity.SizeDisplay = dto.SizeDisplay;
            entity.Color = dto.Color;
            entity.Material = dto.Material;
            entity.Battery = dto.Battery;
            entity.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return "Specification updated successfully.";
        }

        public async Task<bool> DeleteSpecificationAsync(int id)
        {
            var entity = await _context.Specifications.FindAsync(id);
            if (entity == null || entity.IsDelete) return false;

            entity.IsDelete = true;
            entity.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
