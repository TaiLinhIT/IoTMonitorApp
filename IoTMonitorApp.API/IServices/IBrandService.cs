using IoTMonitorApp.API.Models;

namespace IoTMonitorApp.API.IServices
{
    public interface IBrandService
    {
        List<Brand> GetAll();
        Brand GetBrandyById(int id);
        void AddBrand(Brand brand);
        string UpdateBrand(Brand brand);
        bool DeleteBrand(int id);
    }
}
