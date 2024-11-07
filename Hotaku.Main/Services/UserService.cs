using Hotaku.Persistence.Entities;
using Hotaku.Persistence.Repositories;

namespace Hotaku.Main.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }

        public async Task<User?> GetUserById(string userId)
        {
            return await _userRepository.GetUserById(userId);
        }

        public async Task<User> AddUser(User user)
        {
            return await _userRepository.AddUser(user);
        }

        public Task<string> DeleteUser(string userId)
        {
            return _userRepository.DeleteUser(userId);
        }
    }
}
