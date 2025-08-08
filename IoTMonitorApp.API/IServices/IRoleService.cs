using IoTMonitorApp.API.Models;

namespace IoTMonitorApp.API.IServices
{
    public interface IRoleService
    {
        List<Role> GetAll();
        Role GetById(int id);
        void AddRole(Role role);
        string UpdateRole(Role role);
        bool DeleteRole(int id);
    }
}
