using Hotaku.Persistence.Entities;
using Hotaku.Shared.CustomAttribute;
using Microsoft.EntityFrameworkCore;

namespace Hotaku.Persistence.Repositories
{
    [Scoped]
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(HotakuContext hotakuContext) : base(hotakuContext)
        {
        }
        public async Task<User> GetUserById(string id)
        {
            try
            {
                List<User> users = await GetAll();

                User user = users.Find(x => x.UserId == id)
                    ?? throw new InvalidOperationException("User does not exist.");
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<List<User>> GetAllUsers()
        {
            return GetAll();
        }

        public async Task<User> AddUser(User user)
        {
            try
            {
                User? existingUser = GetAll().Result.Find(x => x.Email == user.Email);
                if (existingUser != null)
                {
                    throw new InvalidOperationException("This email is already in use.");
                }

                var newUser = await AddAsync(user);
                return newUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> DeleteUser(User user) {
            try
            {
                return await DeleteAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> UpdateUser(User user)
        {
            try
            {
                var updatedUser = await UpdateAsync(user);
                return updatedUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
