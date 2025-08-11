using IoTMonitorApp.API.Dto.Specification;

namespace IoTMonitorApp.API.IServices
{
    public interface ISpecificationService
    {
        Task<IEnumerable<SpecificationDto>> GetAllAsync();
        Task<SpecificationDto> GetSpecificationByIdAsync(int id);
        Task AddSpecificationAsync(SpecificationDto dto);
        Task<string> UpdateSpecificationAsync(SpecificationDto dto);
        Task<bool> DeleteSpecificationAsync(int id);
    }
}
