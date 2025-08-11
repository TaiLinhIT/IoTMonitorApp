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
            var products = await _dbContext.Products
                .AsNoTracking()
                .Where(p => !p.IsDelete)
                .ToListAsync();

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Slug = p.Slug,
                BrandId = p.BrandId,
                CategoryId = p.CategoryId,
                UserId = p.UserId,
                SpecificationsId = p.SpecificationsId,
                CreatedDate = p.CreatedDate
            });
        }

        public async Task<ProductDto> GetByIdAsync(Guid id)
        {
            var p = await _dbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDelete);

            if (p == null) return null;

            return new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Slug = p.Slug,
                BrandId = p.BrandId,
                CategoryId = p.CategoryId,
                UserId = p.UserId,
                SpecificationsId = p.SpecificationsId,
                CreatedDate = p.CreatedDate
            };
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
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

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                BrandId = product.BrandId,
                CategoryId = product.CategoryId,
                UserId = product.UserId,
                SpecificationsId = product.SpecificationsId,
                CreatedDate = product.CreatedDate
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
