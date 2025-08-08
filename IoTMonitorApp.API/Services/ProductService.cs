using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;

namespace IoTMonitorApp.API.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _dbContext;
        public ProductService(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }
        public string AddProudct(Product product)
        {
            try
            {
                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();
                return "Add successful";

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return "Add fail";
            }
        }

        public string DeleteProduct(Guid id)
        {
            try
            {
                var findProduct = _dbContext.Products.FirstOrDefault(x => x.Id == id);
                if (findProduct == null)
                    return "Not found product";
                if (findProduct != null)
                {
                    findProduct.IsDelete = true;
                    _dbContext.SaveChanges();
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

        public List<Product> GetAll()
        {
            try
            {
                return _dbContext.Products.ToList();

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return new List<Product>();
            }
        }

        public Product GetById(Guid id)
        {
            try
            {
                var findProduct = _dbContext.Products.FirstOrDefault(x => x.Id == id);
                return findProduct;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Product product = new Product();
                return product;
            }
        }

        public string UpdateProduct(Product product)
        {
            try
            {
                var findProduct = _dbContext.Products.FirstOrDefault(x => x.Id == product.Id);
                findProduct.BrandId = product.BrandId;
                findProduct.Name = product.Name;
                findProduct.CategoryId = product.CategoryId;
                findProduct.UpdatedDate = DateTime.Now;
                findProduct.SpecificationsId = product.SpecificationsId;
                _dbContext.Products.Update(findProduct);
                _dbContext.SaveChanges();
                return "Update successful!";

            }
            catch (Exception ex)
            {

                return "Update fail " + ex.Message;
            }
        }
    }
}
