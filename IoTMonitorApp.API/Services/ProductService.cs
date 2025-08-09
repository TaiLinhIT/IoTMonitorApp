using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitorApp.API.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _dbContext;
        public ProductService(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }


        public async Task<string> AddProudctAsync(Product product)
        {
            try
            {
                await _dbContext.Products.AddAsync(product);
                await _dbContext.SaveChangesAsync();
                return "Add successful";

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return "Add fail";
            }
        }


        public async Task<string> DeleteProductAsync(Guid id)
        {
            try
            {
                var findProduct = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
                if (findProduct == null)
                    return "Not found product";
                if (findProduct != null)
                {
                    findProduct.IsDelete = true;
                    await _dbContext.SaveChangesAsync();
                    return "Delete successful";
                }
                return "Delete successful";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Delete fail";
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                return await _dbContext.Products.ToListAsync();

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return new List<Product>();
            }
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            try
            {
                var findProduct = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
                if (findProduct == null)
                {
                    Console.WriteLine("Product not found");
                    return new Product();
                }
                return findProduct;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Product product = new Product();
                return product;
            }
        }



        public async Task<string> UpdateProductAsync(Product product)
        {
            try
            {
                var findProduct = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == product.Id);
                if (findProduct == null)
                    return "Product not found";
                findProduct.BrandId = product.BrandId;
                findProduct.Name = product.Name;
                findProduct.CategoryId = product.CategoryId;
                findProduct.UpdatedDate = DateTime.Now;
                findProduct.SpecificationsId = product.SpecificationsId;
                await _dbContext.SaveChangesAsync();
                return "Update successful!";

            }
            catch (Exception ex)
            {

                return "Update fail " + ex.Message;
            }
        }
    }
}
