using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;

namespace IoTMonitorApp.API.Services
{
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _dbContext;
        public RoleService(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }
        public void AddRole(Role role)
        {
            try
            {
                _dbContext.Roles.Add(role);
                _dbContext.SaveChanges();
                Console.WriteLine("Add successful");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        public bool DeleteRole(int id)
        {
            try
            {
                var findRole = _dbContext.Roles.FirstOrDefault(x => x.Id == id);
                findRole.IsDelete = true;

                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public List<Role> GetAll()
        {
            return _dbContext.Roles.ToList();
        }

        public Role GetById(int id)
        {
            var findRole = _dbContext.Roles.FirstOrDefault(x => x.Id == id);
            return findRole;
        }

        public string UpdateRole(Role role)
        {
            try
            {
                var findRole = _dbContext.Roles.FirstOrDefault(x => x.Id == role.Id);
                findRole.UpdatedDate = DateTime.Now;
                findRole.Description = role.Description;
                findRole.Name = role.Name;
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
