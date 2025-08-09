using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitorApp.API.Services
{
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _dbContext;
        public RoleService(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }


        public async Task AddRoleAsync(Role role)
        {
            try
            {
                await _dbContext.Roles.AddAsync(role);
                await _dbContext.SaveChangesAsync();
                Console.WriteLine("Add successful");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }


        public async Task<bool> DeleteRoleAsync(int id)
        {
            try
            {
                var findRole = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == id);
                if (findRole == null)
                    return false;
                findRole.IsDelete = true;

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _dbContext.Roles.ToListAsync();
        }


        public async Task<Role> GetByIdAsync(int id)
        {
            var findRole = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == id);
            return findRole;
        }


        public async Task<string> UpdateRoleAsync(Role role)
        {
            try
            {
                var findRole = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == role.Id);
                if (findRole == null)
                    return "Role not found";
                findRole.UpdatedDate = DateTime.Now;
                findRole.Description = role.Description;
                findRole.Name = role.Name;
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
