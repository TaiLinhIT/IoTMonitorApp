using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitorApp.API.Services
{
    public class SpecificationService : ISpecificationService
    {
        private readonly AppDbContext _dbContext;
        public SpecificationService(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }


        public async Task AddSpecificationAsync(Specification specification)
        {
            try
            {
                await _dbContext.Specifications.AddAsync(specification);
                await _dbContext.SaveChangesAsync();
                Console.WriteLine("Add successful!");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        public async Task<bool> DeleteSpecificationAsync(int id)
        {
            try
            {
                var findSpecification = await _dbContext.Specifications.FirstOrDefaultAsync(x => x.Id == id);
                if (findSpecification == null)
                    return false;
                findSpecification.IsDelete = true;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Specification>> GetAllAsync()
        {
            return await _dbContext.Specifications.ToListAsync();
        }


        public async Task<Specification> GetSpecificationByIdAsync(int id)
        {
            try
            {
                var findSpecification = await _dbContext.Specifications.FirstOrDefaultAsync(x => x.Id == id);
                if (findSpecification == null)
                    return new Specification();
                return findSpecification;

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return new Specification();
            }
        }


        public async Task<string> UpdateSpecificationAsync(Specification specification)
        {
            try
            {
                var findSpecification = await _dbContext.Specifications.FirstOrDefaultAsync(x => x.Id == specification.Id);
                if (findSpecification == null)
                    return "Not found specification";
                findSpecification.UpdatedDate = specification.UpdatedDate;
                findSpecification.SizeDisplay = specification.SizeDisplay;
                findSpecification.Color = specification.Color;
                findSpecification.Material = specification.Material;
                findSpecification.Battery = specification.Battery;
                findSpecification.Storage = specification.Storage;
                await _dbContext.SaveChangesAsync();
                return "Update successful";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }
    }
}
