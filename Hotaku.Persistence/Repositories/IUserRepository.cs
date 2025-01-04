using Hotaku.Persistence.Entities;

namespace Hotaku.Persistence.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserById(string id);

        Task<List<User>> GetAllUsers();

        Task<User> AddUser(User user);

        Task<string> DeleteUser(User user);

        Task<User> UpdateUser(User user);
    }
}
