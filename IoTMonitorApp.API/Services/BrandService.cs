using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.Dto;
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


        public async Task AddBrandAsync(BrandDto brand)
        {
            try
            {
                var dto = new Brand
                {
                    Name = brand.Name,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    IsDelete = false
                };
                await _dbContext.Brands.AddAsync(dto);
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

        public async Task<string> UpdateBrandAsync(int id, BrandDto dto)
        {
            try
            {
                var findBrand = await _dbContext.Brands.FirstOrDefaultAsync(b => b.Id == id);
                if (findBrand == null)
                    return "Brand not found";
                findBrand.UpdatedDate = DateTime.Now;
                findBrand.Name = dto.Name;
                await _dbContext.SaveChangesAsync();
                return "Update successful";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }

        Task<IEnumerable<BrandDto>> IBrandService.GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
