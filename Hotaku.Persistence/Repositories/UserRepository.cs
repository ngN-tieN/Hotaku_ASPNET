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

        public async Task<List<User>> GetAllUsers()
        {
            return await GetAll().ToListAsync();
        }

        public async Task<User> AddUser(User user)
        {
            try
            {
                var existingUser = await GetAll().FirstOrDefaultAsync(x => x.Email == user.Email);
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

        public async Task<string> DeleteUser(string id)
        {
            try
            {
                var user = await HotakuContext.Set<User>()
                    .Include(u => u.UserFavoriteMangas)
                    .Include(u => u.UserMangaHistories)
                    .Include(u => u.Notifications)
                    .FirstOrDefaultAsync(u => u.UserId == id)
                    ?? throw new InvalidOperationException("User does not exist.");

                HotakuContext.UserFavoriteMangas.RemoveRange(user.UserFavoriteMangas);
                HotakuContext.UserMangaHistories.RemoveRange(user.UserMangaHistories);

                foreach (var notification in user.Notifications)
                {
                    notification.Users.Remove(user);
                }

                HotakuContext.Set<User>().Remove(user);
                await HotakuContext.SaveChangesAsync();

                return "User deleted successfully.";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
