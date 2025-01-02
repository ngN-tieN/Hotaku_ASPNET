using Hotaku.Persistence.Entities;

namespace Hotaku.Main.Services
{
    public interface IUserService
    {
        Task<User?> GetUserById(string userId);

        List<User> GetAllUsers();

        Task<User> AddUser(User user);

        Task<string> DeleteUser(User user);

        Task<User> UpdateUser(User user);
    }
}
