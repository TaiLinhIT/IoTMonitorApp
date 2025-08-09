using IoTMonitorApp.API.Models;

namespace IoTMonitorApp.API.IServices
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role> GetByIdAsync(int id);
        Task AddRoleAsync(Role role);
        Task<string> UpdateRoleAsync(Role role);
        Task<bool> DeleteRoleAsync(int id);
    }
}
