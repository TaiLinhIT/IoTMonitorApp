using IoTMonitorApp.API.Models;

namespace IoTMonitorApp.API.IServices
{
    public interface IProductService
    {
        List<Product> GetAll();
        Product GetById(Guid id);
        string AddProudct(Product product);
        string UpdateProduct(Product product);
        string DeleteProduct(Guid id);
    }
}
