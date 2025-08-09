using IoTMonitorApp.API.Data;
using IoTMonitorApp.API.IServices;
using IoTMonitorApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitorApp.API.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        public UserService(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }
        public async Task AddUserAsync(User user)
        {
            try
            {
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                Console.WriteLine("Add successful!");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var findUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (findUser == null)
                    return false;
                findUser.IsDelete = true;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                    return new User();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new User();
            }

        }

        public async Task<string> UpdateUserAsync(User user)
        {
            try
            {
                var userFind = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
                if (userFind == null)
                    return "User not found";
                userFind.FullName = user.FullName;
                userFind.Email = user.Email;
                userFind.ImageUrl = user.ImageUrl;
                userFind.Role = user.Role;
                userFind.UpdatedDate = DateTime.UtcNow;
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
