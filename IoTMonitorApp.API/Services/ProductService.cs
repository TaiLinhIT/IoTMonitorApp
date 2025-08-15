using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.Dto.Proudct;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitorApp.API.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _dbContext;

        public ProductService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await (from p in _dbContext.Products
                                  join b in _dbContext.Brands on p.BrandId equals b.Id
                                  join c in _dbContext.Categories on p.CategoryId equals c.Id
                                  join s in _dbContext.Specifications on p.SpecificationsId equals s.Id
                                  where !p.IsDelete
                                  select new ProductDto
                                  {
                                      Id = p.Id,
                                      Name = p.Name,
                                      Slug = p.Slug,
                                      BrandName = b.Name,
                                      CategoryName = c.Name,
                                      SpecificationsName = s.Material,
                                      Price = p.Price,
                                      ImageUrl = p.ImageUrl,
                                      CreatedDate = p.CreatedDate
                                  }).ToListAsync();

            return products;
        }

        public async Task<ProductDto> GetByIdAsync(Guid id)
        {
            var Findproduct = await (from p in _dbContext.Products
                                     join b in _dbContext.Brands on p.BrandId equals b.Id
                                     join c in _dbContext.Categories on p.CategoryId equals c.Id
                                     join s in _dbContext.Specifications on p.SpecificationsId equals s.Id
                                     where p.Id == id && !p.IsDelete
                                     select new ProductDto
                                     {
                                         Id = p.Id,
                                         Name = p.Name,
                                         Slug = p.Slug,
                                         BrandName = b.Name,
                                         CategoryName = c.Name,
                                         SpecificationsName = s.Material,
                                         Price = p.Price,
                                         ImageUrl = p.ImageUrl,
                                         CreatedDate = p.CreatedDate
                                     }).FirstOrDefaultAsync();

            if (Findproduct == null) return null;

            return Findproduct;
        }

        public async Task<CreateProductDto> CreateAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                BrandId = dto.BrandId,
                CategoryId = dto.CategoryId,
                UserId = dto.UserId,
                SpecificationsId = dto.SpecificationsId,
                CreatedDate = DateTime.UtcNow,
                IsDelete = false
            };

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            return new CreateProductDto
            {
                Name = product.Name,
                BrandId = product.BrandId,
                CategoryId = product.CategoryId,
                UserId = product.UserId,
                SpecificationsId = product.SpecificationsId
            };

        }

        public async Task<bool> UpdateAsync(UpdateProductDto dto)
        {
            var product = await _dbContext.Products.FindAsync(dto.Id);
            if (product == null || product.IsDelete) return false;

            product.Name = dto.Name;
            product.BrandId = dto.BrandId;
            product.CategoryId = dto.CategoryId;
            product.UserId = dto.UserId;
            product.SpecificationsId = dto.SpecificationsId;
            product.UpdatedDate = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null || product.IsDelete) return false;

            product.IsDelete = true;
            product.UpdatedDate = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
