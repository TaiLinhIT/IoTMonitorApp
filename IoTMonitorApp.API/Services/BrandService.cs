using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;

namespace IoTMonitorApp.API.Services
{
    public class BrandService : IBrandService
    {
        private readonly AppDbContext _dbContext;
        public BrandService(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }
        public void AddBrand(Brand brand)
        {
            try
            {
                _dbContext.Brands.Add(brand);
                _dbContext.SaveChanges();
                Console.WriteLine("Add successful!");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        public bool DeleteBrand(int id)
        {
            try
            {
                var findBrand = _dbContext.Brands.FirstOrDefault(b => b.Id == id);
                findBrand.IsDelete = true;
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public List<Brand> GetAll()
        {
            return _dbContext.Brands.ToList();
        }

        public Brand GetBrandyById(int id)
        {
            try
            {
                var findBrand = _dbContext.Brands.FirstOrDefault(b => b.Id == id);
                return findBrand;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Brand();
            }
        }

        public string UpdateBrand(Brand brand)
        {
            try
            {
                var findBrand = _dbContext.Brands.FirstOrDefault(b => b.Id == brand.Id);
                findBrand.UpdatedDate = DateTime.Now;
                findBrand.Name = brand.Name;
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
