using Hotaku.Persistence.Entities;
using Hotaku.Persistence.Repositories;
using Hotaku.Shared.CustomAttribute;

namespace Hotaku.Main.Services
{
    [Scoped]
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<List<User>> GetAllUsers()
        {
            var users = _userRepository.GetAllUsers();
            return users;
        }

        public async Task<User> GetUserById(string userId)
        {
            var user = await _userRepository.GetUserById(userId);
            return user;
        }

        public async Task<User> AddUser(User user)
        {
            return await _userRepository.AddUser(user);
        }

        public async Task<string> DeleteUser(User user)
        {
            return await _userRepository.DeleteUser(user);
        }

        public Task<User> UpdateUser(User user)
        {
            return _userRepository.UpdateUser(user);
        }
    }
}
