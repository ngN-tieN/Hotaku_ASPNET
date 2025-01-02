using Hotaku.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hotaku.Persistence.Repositories
{
    public class UserRepository(HotakuContext hotakuContext) : Repository<User>(hotakuContext), IUserRepository
    {
        public async Task<User> GetUserById(string id)
        {
            try
            {
                var user = await GetAll().FirstOrDefaultAsync(x => x.UserId == id)
               ?? throw new InvalidOperationException("User does not exist.");
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<User> GetAllUsers()
        {
            return [.. GetAll()];
        }

        public async Task<User> AddUser(User user)
        {
            try
            {
                List<User> users = [.. GetAll()];
                var existingUser = users.Find(x => x.Email == user.Email);
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

        public async Task<string> DeleteUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            try
            {
                hotakuContext.Remove(user);
                await hotakuContext.SaveChangesAsync();
                return $"User with ID '{user.UserId}' has been successfully deleted.";
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting user: {ex.Message}");
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
                throw new Exception($"Error deleting user: {ex.Message}");
            }
        }
    }
}
