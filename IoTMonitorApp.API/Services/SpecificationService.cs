using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;

namespace IoTMonitorApp.API.Services
{
    public class SpecificationService : ISpecificationService
    {
        private readonly AppDbContext _dbContext;
        public SpecificationService(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }
        public void AddSpecification(Specification specification)
        {
            try
            {
                _dbContext.Specifications.Add(specification);
                _dbContext.SaveChanges();
                Console.WriteLine("Add successful!");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        public bool DeleteSpecification(int id)
        {
            try
            {
                var findSpecification = _dbContext.Specifications.FirstOrDefault(x => x.Id == id);
                findSpecification.IsDelete = true;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public List<Specification> GetAll()
        {
            return _dbContext.Specifications.ToList();
        }

        public Specification GetSpecificationById(int id)
        {
            try
            {
                var findSpecification = _dbContext.Specifications.FirstOrDefault(x => x.Id == id);
                return findSpecification;

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return new Specification();
            }
        }

        public string UpdateSpecification(Specification specification)
        {
            try
            {
                var findSpecification = _dbContext.Specifications.FirstOrDefault(x => x.Id == specification.Id);
                findSpecification.UpdatedDate = specification.UpdatedDate;
                findSpecification.SizeDisplay = specification.SizeDisplay;
                findSpecification.Color = specification.Color;
                findSpecification.Material = specification.Material;
                findSpecification.Battery = specification.Battery;
                findSpecification.Storage = specification.Storage;
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
