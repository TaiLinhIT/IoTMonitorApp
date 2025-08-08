using IoTMonitorApp.API.Models;

namespace IoTMonitorApp.API.IServices
{
    public interface ISpecificationService
    {
        List<Specification> GetAll();
        Specification GetSpecificationById(int id);
        void AddSpecification(Specification specification);
        string UpdateSpecification(Specification specification);
        bool DeleteSpecification(int id);
    }
}
