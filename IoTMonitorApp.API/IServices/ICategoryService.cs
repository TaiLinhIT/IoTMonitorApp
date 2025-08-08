using IoTMonitorApp.API.Models;

namespace IoTMonitorApp.API.IServices
{
    public interface ICategoryService
    {
        List<Category> GetAll();
        Category GetCategoryById(int id);
        void AddCategory(Category category);
        string UpdateCategory(Category category);
        bool DeleteCategory(int id);
    }
}
