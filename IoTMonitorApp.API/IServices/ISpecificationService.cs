using IoTMonitorApp.API.Models;

namespace IoTMonitorApp.API.IServices
{
    public interface ISpecificationService
    {
        Task<IEnumerable<Specification>> GetAllAsync();
        Task<Specification> GetSpecificationByIdAsync(int id);
        Task AddSpecificationAsync(Specification specification);
        Task<string> UpdateSpecificationAsync(Specification specification);
        Task<bool> DeleteSpecificationAsync(int id);
    }
}
