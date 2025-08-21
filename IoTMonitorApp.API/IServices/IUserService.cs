using IoTMonitorApp.API.Models;

namespace IoTMonitorApp.API.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(Guid id);
        Task AddUserAsync(User user);
        Task<string> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(Guid id);

    }
}
