using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitorApp.API.Services
{
    public class BrandService : IBrandService
    {
        private readonly AppDbContext _dbContext;
        public BrandService(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }


        public async Task AddBrandAsync(Brand brand)
        {
            try
            {
                await _dbContext.Brands.AddAsync(brand);
                await _dbContext.SaveChangesAsync();
                Console.WriteLine("Add successful!");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }


        public async Task<bool> DeleteBrandAsync(int id)
        {
            try
            {
                var findBrand = await _dbContext.Brands.FirstOrDefaultAsync(b => b.Id == id);
                if (findBrand == null)
                    return false;
                findBrand.IsDelete = true;
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            return await _dbContext.Brands.ToListAsync();
        }


        public async Task<Brand> GetBrandyByIdAsync(int id)
        {
            try
            {
                var findBrand = await _dbContext.Brands.FirstOrDefaultAsync(b => b.Id == id);
                return findBrand;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Brand();
            }
        }


        public async Task<string> UpdateBrandAsync(Brand brand)
        {
            try
            {
                var findBrand = await _dbContext.Brands.FirstOrDefaultAsync(b => b.Id == brand.Id);
                if (findBrand == null)
                    return "Brand not found";
                findBrand.UpdatedDate = DateTime.Now;
                findBrand.Name = brand.Name;
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
