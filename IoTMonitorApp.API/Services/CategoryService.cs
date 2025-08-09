using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitorApp.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _dbContext;
        public CategoryService(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }


        public async Task AddCategoryAsync(Category category)
        {
            try
            {
                await _dbContext.Categories.AddAsync(category);
                await _dbContext.SaveChangesAsync();
                Console.WriteLine("Add successful!");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }


        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                var findCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
                if (findCategory == null)
                    return false;
                findCategory.IsDelete = true;
                await _dbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var category = await _dbContext.Categories.ToListAsync();
            return category;
        }


        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            try
            {
                var findCategory = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (findCategory == null)
                {
                    Console.WriteLine("Category not found");
                    return new Category();
                }
                return findCategory;

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return new Category();
            }
        }

        public async Task<string> UpdateCategoryAsync(Category category)
        {
            try
            {
                var findCategory = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
                if (findCategory == null)
                    return "Category not found";
                findCategory.UpdatedDate = DateTime.Now;
                findCategory.Name = category.Name;
                await _dbContext.SaveChangesAsync();
                return "Update successful";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }
    }
}
