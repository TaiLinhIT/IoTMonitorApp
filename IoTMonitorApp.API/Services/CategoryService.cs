using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;

namespace IoTMonitorApp.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _dbContext;
        public CategoryService(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }
        public void AddCategory(Category category)
        {
            try
            {
                _dbContext.Categories.Add(category);
                _dbContext.SaveChanges();
                Console.WriteLine("Add successful!");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        public bool DeleteCategory(int id)
        {
            try
            {
                var findCategory = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
                findCategory.IsDelete = true;
                _dbContext.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public List<Category> GetAll()
        {
            var category = _dbContext.Categories.ToList();
            return category;
        }

        public Category GetCategoryById(int id)
        {
            try
            {
                var findCategory = _dbContext.Categories.FirstOrDefault(x => x.Id == id);
                return findCategory;

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return new Category();
            }
        }

        public string UpdateCategory(Category category)
        {
            try
            {
                var findCategory = _dbContext.Categories.FirstOrDefault(x => x.Id == category.Id);
                findCategory.UpdatedDate = DateTime.Now;
                findCategory.Name = category.Name;
                _dbContext.SaveChanges();
                return "Update successful";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }
    }
}
