using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.Dto;
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


        public async Task AddCategoryAsync(CategoryDto category)
        {
            try
            {
                var Catetory = new Category
                {
                    Name = category.Name,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    IsDelete = false
                };
                await _dbContext.Categories.AddAsync(Catetory);
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



        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _dbContext.Categories.ToListAsync();
            var dto = categories.Select(c => new CategoryDto
            {
                Name = c.Name
            });
            return dto;
        }



        public async Task<string> UpdateCategoryAsync(int id, CategoryDto category)
        {
            try
            {
                var findCategory = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
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



        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            try
            {
                var findCategory = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (findCategory == null)
                {
                    Console.WriteLine("Category not found");
                    return new CategoryDto();
                }
                var categoryDto = new CategoryDto
                {
                    Name = findCategory.Name
                };
                return categoryDto;

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return new CategoryDto();
            }
        }
    }
}
