using IoTMonitorApp.API.Models;

namespace IoTMonitorApp.API.IServices
{
    public interface IBrandService
    {
        Task<IEnumerable<Brand>> GetAllAsync();
        Task<Brand> GetBrandyByIdAsync(int id);
        Task AddBrandAsync(Brand brand);
        Task<string> UpdateBrandAsync(Brand brand);
        Task<bool> DeleteBrandAsync(int id);
    }
}
