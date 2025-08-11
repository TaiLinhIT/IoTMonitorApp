using IoTMonitorApp.API.Dto;

namespace IoTMonitorApp.API.IServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(CategoryDto category);
        Task<string> UpdateCategoryAsync(int id, CategoryDto category);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
