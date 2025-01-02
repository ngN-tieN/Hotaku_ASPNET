using Hotaku.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Hotaku.Shared.CustomAttribute;
namespace Hotaku.Persistence.Repositories
{
    [Scoped]
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly HotakuContext HotakuContext;

        public Repository(HotakuContext hotakuContext)
        {
            HotakuContext = hotakuContext;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            try
            {
                await HotakuContext.AddAsync(entity);
                await HotakuContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            try
            {
                // Attach the entity to the context if it's not already being tracked
                var entry = HotakuContext.Entry(entity);

                var key = HotakuContext.Model.FindEntityType(typeof(TEntity))?.FindPrimaryKey()?.Properties
                    .Select(p => typeof(TEntity).GetProperty(p.Name)?.GetValue(entity));

                if (key != null)
                {
                    var existingEntity = await HotakuContext.Set<TEntity>().FindAsync(key);
                    entry.State = EntityState.Modified;
                    if (existingEntity != null)
                    {
                        HotakuContext.Entry(existingEntity).CurrentValues.SetValues(entity);
                    }
                    else
                    {
                        throw new Exception($"{nameof(entity)} with the specified key does not exist.");
                    }
                }

                // Save changes to the database
                await HotakuContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
            }
        }

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return HotakuContext.Set<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }
    }
}
