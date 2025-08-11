using IoTMonitorApp.API.Dto;
using IoTMonitorApp.API.Models;

namespace IoTMonitorApp.API.IServices
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandDto>> GetAllAsync();
        Task<Brand> GetBrandyByIdAsync(int id);
        Task AddBrandAsync(BrandDto dto);
        Task<string> UpdateBrandAsync(int id, BrandDto dto);
        Task<bool> DeleteBrandAsync(int id);
    }
}
