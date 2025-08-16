using IoTMonitorApp.API.Dto.Proudct;

namespace IoTMonitorApp.API.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        //Task<ProductDto> GetByIdAsync(Guid id);
        Task<CreateProductDto> CreateAsync(CreateProductDto dto);
        Task<bool> UpdateAsync(UpdateProductDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<ProductItemDto> GetProductItem(Guid id);
    }

}
